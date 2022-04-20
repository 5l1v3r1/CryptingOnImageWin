using System.Diagnostics;
using System.Globalization;
using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace CryptingOnImageWin
{
	public partial class Form1 : Form
	{
		OpenFileDialog fileDialog;

		public Form1()
		{
			InitializeComponent();
		}
		private static string _privateKey;
		private static string _publicKey;
		private static UnicodeEncoding _encoder = new UnicodeEncoding();

		private static void RSA()
		{
			var rsa = new RSACryptoServiceProvider();
			_privateKey = rsa.ToXmlString(true);
			_publicKey = rsa.ToXmlString(false);

			var text = "Test1";
			Console.WriteLine("RSA // Text to encrypt: " + text);
			var enc = Encrypt(text);
			Console.WriteLine("RSA // Encrypted Text: " + enc);
			var dec = Decrypt(enc);
			Console.WriteLine("RSA // Decrypted Text: " + dec);
		}

		public static string Decrypt(string data)
		{
			var rsa = new RSACryptoServiceProvider();
			var dataArray = data.Split(new char[] { ',' });
			byte[] dataByte = new byte[dataArray.Length];
			for (int i = 0; i < dataArray.Length; i++)
			{
				dataByte[i] = Convert.ToByte(dataArray[i]);
			}

			rsa.FromXmlString(_privateKey);
			var decryptedByte = rsa.Decrypt(dataByte, false);
			return _encoder.GetString(decryptedByte);
		}

		public static string Encrypt(string data)
		{
			var rsa = new RSACryptoServiceProvider();
			rsa.FromXmlString(_publicKey);
			var dataToEncrypt = _encoder.GetBytes(data);
			var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
			var length = encryptedByteArray.Count();
			var item = 0;
			var sb = new StringBuilder();
			foreach (var x in encryptedByteArray)
			{
				item++;
				sb.Append(x);
				if (item < length)
					sb.Append(",");
			}

			return sb.ToString();
		}

		private void Btn_ResimSec_Click(object sender, EventArgs e)
		{
			openFileDialog1.DefaultExt = "*.*";
			openFileDialog1.Filter = "Tüm dosyalar (*.*)|*.*";

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Bitmap bmp = new Bitmap(openFileDialog1.FileName);
				//Task.Run(() => ImageConvertToBasicImage6(bmp, richTextBox1));
				ImageConvertToBasicImage6_2(bmp, richTextBox1.Text);
			}
		}

		private void Btn_ResimOku_Click(object sender, EventArgs e)
		{
			openFileDialog1.DefaultExt = "*.*";
			openFileDialog1.Filter = "Tüm dosyalar (*.*)|*.*";

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Bitmap bmp = new Bitmap(openFileDialog1.FileName);
				//Task.Run(() => ReadImage(bmp));
				ReadImage(bmp);
			}
		}

		private char[] StringToArrayChar(string dataStr)
		{
			char[] chars = new char[dataStr.Length];
			for (int i = 0; i < dataStr.Length; i++)
			{
				char a = Convert.ToChar(dataStr[i]);
				chars.Append(a);
			}
			return chars;
		}

		private byte[] StringBinaryToByteArray(string stringBinary)
		{
			int numOfBytes = stringBinary.Length / 8;
			byte[] bytes = new byte[numOfBytes];
			for (int i = 0; i < numOfBytes; ++i)
			{
				bytes[i] = Convert.ToByte(stringBinary.Substring(8 * i, 8), 2);
			}
			return bytes;
		}

		private short[] StringBinaryToShorts(string stringBinary)
		{
			int numOfShorts = stringBinary.Length / 16;
			short[] shorts = new short[numOfShorts];
			for (int i = 0; i < numOfShorts; ++i)
			{
				shorts[i] = Convert.ToInt16(stringBinary.Substring(16 * i, 16), 2);
			}
			return shorts;
		}

		private byte[] CharToBytes(char ch)
		{
			return BitConverter.GetBytes(ch);
		}

		private short CharToShort(char ch)
		{
			return Convert.ToInt16(ch);
		}

		public String ToBinary(byte[] data)
		{
			return string.Join("", data.Select(byt => Convert.ToString(byt, 2).PadLeft(8, '0')));
		}

		public string LastBitsToString(byte[] binary)
		{
			string s = "";
			foreach (var c in binary)
			{
				char cCc = ToBinary(new byte[] { c }).Last();
				s += cCc.ToString();
			}
			return s;
		}

		public String To16Bit(short[] data)
		{
			return string.Join("", data.Select(shrt => Convert.ToString(shrt, 2).PadLeft(16, '0')));
		}

		private void ImageConvertToBasicImage6(Bitmap bitmap1, string dataStr)
		{
			dataStr = ConvertTurkishCharsToEnglish(dataStr);
			var chars = dataStr.ToCharArray();
			int pixelCount = bitmap1.Width * bitmap1.Height;

			if (chars.Length * 8 > pixelCount)
			{
				MessageBox.Show("Uyarı!", "Şifrelenecek data image içerisine sığmıyor", MessageBoxButtons.OK);
				return;
			}

			for (int i = 0; i < chars.Length; i++)
			{
				bool Isi = true;
				if (chars[i] == '■')
				{
					break;
				}
				for (int x = 0; x < bitmap1.Width; x++)
				{
					if (x != 0 && x % bitmap1.Width == 0)
						break;
					for (int y = 0; y < bitmap1.Height; y++)
					{
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r = bitmap1.GetPixel(x, y).R;
						byte g = bitmap1.GetPixel(x, y).G;
						byte b = bitmap1.GetPixel(x, y).B;
						y++;
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r1 = bitmap1.GetPixel(x, y).R;
						byte g1 = bitmap1.GetPixel(x, y).G;
						byte b1 = bitmap1.GetPixel(x, y).B;
						y++;
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r2 = bitmap1.GetPixel(x, y).R;
						byte g2 = bitmap1.GetPixel(x, y).G;
						byte b2 = bitmap1.GetPixel(x, y).B;

						byte[] renkler = new byte[] { r, g, b, r1, g1, b1, r2, g2, b2 };

						if (Isi)
						{
							byte[] writeCharBytes = BitConverter.GetBytes(chars[i]);
							string eklenecekCharBinary = ToBinary(writeCharBytes);
							byte[] yeniByteData = new byte[renkler.Length];
							for (int q = 0; q < yeniByteData.Length; q++)
							{
								byte[] writeByte = BitConverter.GetBytes(renkler[q]);
								string aklenecekBinary = ToBinary(writeByte);
								//char[] changeChars = aklenecekBinary.ToCharArray();
								string firstData1 = aklenecekBinary.Substring(0, 15);
								//string firstData2 = aklenecekBinary.Substring(0, 7);
								string yenidata = firstData1 + eklenecekCharBinary.Substring(15, 1);// hata burda
								byte yeniBytes = StringBinaryToByteArray(yenidata).First();
								yeniByteData[q] = yeniBytes;
							}
							y -= 2;
							r = yeniByteData[0];
							g = yeniByteData[1];
							b = yeniByteData[2];
							bitmap1.SetPixel(x, y, Color.FromArgb(r, g, b));
							y++;
							if (y != 0 && y % bitmap1.Height == 0)
								break;
							r1 = yeniByteData[3];
							g1 = yeniByteData[4];
							b1 = yeniByteData[5];
							bitmap1.SetPixel(x, y, Color.FromArgb(r1, g1, b1));
							y++;
							if (y != 0 && y % bitmap1.Height == 0)
								break;
							r2 = yeniByteData[6];
							g2 = yeniByteData[7];
							b2 = yeniByteData[8];
							bitmap1.SetPixel(x, y, Color.FromArgb(r2, g2, b2));
							Isi = false;
						}
					}
				}
			}

			string fileName = Txt_Name.Text + ".png";
			bitmap1.Save(fileName);
			OpenImage(fileName);
			bitmap1.Dispose();
			MessageBox.Show("Başarılı", "Tebrikler! İşlem başarılı", MessageBoxButtons.OK);
		}
		private void ImageConvertToBasicImage6_2(Bitmap bitmap1, string dataStr)
		{
			dataStr = ConvertTurkishCharsToEnglish(dataStr);
			char[] chars = dataStr.ToCharArray();
			int pixelCount = bitmap1.Width * bitmap1.Height;
			int dataCount = chars.Length;
			if (dataCount * 24 > pixelCount)
			{
				MessageBox.Show("Uyarı!", "Şifrelenecek data image içerisine sığmıyor", MessageBoxButtons.OK);
				return;
			}
			//byte[] dataBtesArr = BitConverter.GetBytes(dataCount);
			//int n = Convert.ToInt32(ToBinary(dataBtesArr),2);
			bitmap1.SetPixel(0, 0, Color.Empty);
			bitmap1.SetPixel(0, 0, Color.FromArgb(dataCount));

			//string fileName = @"C:\Users\AKR3P\source\repos\CryptingOnImageWin\CryptingOnImageWin\bin\Debug\net6.0-windows";
			//bitmap1.Save(Directory.GetFiles(fileName).FirstOrDefault("daghan.png"));
			//bitmap1.Dispose();
			//MessageBox.Show(">>  R:" + bitmap1.GetPixel(0, 0).R + " G:" + bitmap1.GetPixel(0, 0).G + " B:" + bitmap1.GetPixel(0, 0).B+" ToArgb:"+bitmap1.GetPixel(0,0).ToArgb(), "Tamam Tamam Tebrikler! ilk adımı geçtiniz >>", MessageBoxButtons.OK);

			for (int i = 0; i < dataCount; i++)
			{
				bool Isi = true;
				//if (chars[i] == '■')
				//{
				//	break;
				//}
				for (int x = 0; x < bitmap1.Width; x++)
				{
					if (x != 0 && x % bitmap1.Width == 0)
						break;
					for (int y = 1; y < bitmap1.Height; y++)
					{
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r = bitmap1.GetPixel(x, y).R;
						byte g = bitmap1.GetPixel(x, y).G;
						byte b = bitmap1.GetPixel(x, y).B;
						y++;
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r1 = bitmap1.GetPixel(x, y).R;
						byte g1 = bitmap1.GetPixel(x, y).G;
						byte b1 = bitmap1.GetPixel(x, y).B;
						y++;
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r2 = bitmap1.GetPixel(x, y).R;
						byte g2 = bitmap1.GetPixel(x, y).G;
						byte b2 = bitmap1.GetPixel(x, y).B;

						byte[] renkler = new byte[] { r, g, b, r1, g1, b1, r2, g2, b2 };

						if (Isi)
						{
							byte writeCharBytes = Convert.ToByte(chars[i]);
							char[] eklenecekCharBinary = ToBinary(new byte[] { writeCharBytes }).ToCharArray();
							byte[] yeniByteData = new byte[renkler.Length];
							string[] yeniStringData = new string[renkler.Length];
							for (int q = 0; q < yeniByteData.Length - 1; q++)
							{

								//if (q < 8)
								//{
								string strBnry = ToBinary(new byte[] { renkler[q] }).Substring(0, 7);
								//byte[] writeByte = BitConverter.GetBytes(renkler[q]);
								//string aklenecekBinary = ToBinary(writeByte);
								////char[] changeChars = aklenecekBinary.ToCharArray();
								////string firstData1 = aklenecekBinary.Substring(0, 7);
								//string firstData1 = aklenecekBinary.Substring(0, 7);
								//string firstData2 = aklenecekBinary.Substring(0, 7);
								string yenidata = strBnry + eklenecekCharBinary[q];//.Substring(q, 1);// hata burda
								yeniStringData[q] = yenidata;
								//byte yeniBytes = StringBinaryToByteArray(yenidata).First();
								//yeniByteData[q] = yeniBytes;
								//byte yeniByte = Convert.ToByte(eklenecekCharBinary, 2);
								yeniByteData[q] = Convert.ToByte(yenidata, 2);
								//}
								//else
								//{
								//	yeniStringData[q] = "";
								//	yeniByteData[q] = renkler[q];
								//}								
							}
							yeniStringData[8] = ToBinary(new byte[] { yeniByteData[8] });
							yeniByteData[8] = renkler[8];

							y -= 2;
							r = yeniByteData[0];
							g = yeniByteData[1];
							b = yeniByteData[2];
							bitmap1.SetPixel(x, y, Color.FromArgb(r, g, b));
							y++;
							if (y != 0 && y % bitmap1.Height == 0)
								break;
							r1 = yeniByteData[3];
							g1 = yeniByteData[4];
							b1 = yeniByteData[5];
							bitmap1.SetPixel(x, y, Color.FromArgb(r1, g1, b1));
							y++;
							if (y != 0 && y % bitmap1.Height == 0)
								break;
							r2 = yeniByteData[6];
							g2 = yeniByteData[7];
							b2 = yeniByteData[8];
							bitmap1.SetPixel(x, y, Color.FromArgb(r2, g2, b2));
							Isi = false;
						}
						if (chars[i] == '+')
						{
							break;
						}
					}
					if (chars[i] == '+')
					{
						break;
					}
				}
				if (chars[i] == '+')
				{
					break;
				}
			}
			string fileName = Txt_Name.Text + ".png";

			bitmap1.Save(fileName);
			//OpenImage(fileName);
			//bitmap1.Dispose();
			MessageBox.Show("Başarılı", "Tebrikler! İşlem başarılı", MessageBoxButtons.OK);
			ReadImage(bitmap1);
		}

		private void ReadImage(Bitmap bitmap1)
		{
			richTextBox1.Text += "\n---\n";

			byte R = bitmap1.GetPixel(0, 0).R;
			byte G = bitmap1.GetPixel(0, 0).G;
			byte B = bitmap1.GetPixel(0, 0).B;

			//string Sr = ToBinary(new byte[] { R });
			//string Sg = ToBinary(new byte[] { G });
			//string Sb = ToBinary(new byte[] { B });
			string RGB = ToBinary(new byte[] { R, G, B });
			int RGBint = Convert.ToInt32(RGB, 2);
			//char[] RGBcharr = RGB.ToCharArray();
			//int RGBLength = RGB.Length;

			//int Ir = Convert.ToInt32(Sr, 2);
			//int Ig = Convert.ToInt32(Sg, 2);
			//int Ib = Convert.ToInt32(Sb, 2);

			//int count = Convert.ToInt32(RGB);
			//------------------------------
			for (int i = 0; i < RGBint; i++)
			{
				bool IsLastChar = false;
				for (int x = 0; x < bitmap1.Width; x++)
				{
					if (x != 0 && x % bitmap1.Width == 0)
						break;
					for (int y = 1; y < bitmap1.Height; y++)
					{
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r = bitmap1.GetPixel(x, y).R;
						byte g = bitmap1.GetPixel(x, y).G;
						byte b = bitmap1.GetPixel(x, y).B;
						y++;
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r1 = bitmap1.GetPixel(x, y).R;
						byte g1 = bitmap1.GetPixel(x, y).G;
						byte b1 = bitmap1.GetPixel(x, y).B;
						y++;
						if (y != 0 && y % bitmap1.Height == 0)
							break;
						byte r2 = bitmap1.GetPixel(x, y).R;
						byte g2 = bitmap1.GetPixel(x, y).G;
						byte b2 = bitmap1.GetPixel(x, y).B;
						//							  1  0	1	0	1	0	1	1	1
						byte[] renkler = new byte[] { r, g, b, r1, g1, b1, r2, g2, b2 };
						char[] renklerStr = new char[] {
							//ToBinary(new byte[] { r })
							//.Substring(7,1), // 0
							//ToBinary(new byte[] { g })
							//.Substring(7,1), // 1
							//ToBinary(new byte[] { b })
							//.Substring(7,1), // 2
							//ToBinary(new byte[] { r1 })
							//.Substring(7,1), // 3
							//ToBinary(new byte[] { g1 })
							//.Substring(7,1), // 4
							//ToBinary(new byte[] { b1 })
							//.Substring(7,1), // 5
							//ToBinary(new byte[] { r2 })
							//.Substring(7,1), // 6
							//ToBinary(new byte[] { g2 })
							//.Substring(7,1), // 7
							//ToBinary(new byte[] { b2 })
							//.Substring(7,1), // 8
							ToBinary(new byte[] { r }).Last(),
							ToBinary(new byte[] { g }).Last(),
							ToBinary(new byte[] { b }).Last(),
							ToBinary(new byte[] { r1 }).Last(),
							ToBinary(new byte[] { g1 }).Last(),
							ToBinary(new byte[] { b1 }).Last(),
							ToBinary(new byte[] { r2 }).Last(),
							ToBinary(new byte[] { g2 }).Last(),
							ToBinary(new byte[] { b2 }).Last(),
							};
						string totalStr = "";

						for (int t = 0; t < renkler.Length - 1; t++)
						{
							totalStr += renklerStr[t];
						}
						int number = Convert.ToInt32(totalStr, 2);
						richTextBox1.Text += Convert.ToChar(number);
						if (number == 43)
						{
							IsLastChar = true;
							break;
						}
					}
					if (IsLastChar)
					{
						break;
					}
				}
				if (IsLastChar)
				{
					break;
				}
			}
			MessageBox.Show("Başarılı", "Tebrikler! İşlem başarılı", MessageBoxButtons.OK);
		}

		private List<byte> ReaderImageByte(Bitmap bitmap1)
		{
			bool IsFinished = false;
			List<byte> renkler = new List<byte>();
			for (int y = 0; y < bitmap1.Height; y++)
			{
				if (IsFinished)
				{
					break;
				}
				for (int x = 0; x < bitmap1.Width; x++)
				{
					if (IsFinished)
					{
						break;
					}
					if (y != 0 && y % bitmap1.Height == 0)
						break;
					byte r = bitmap1.GetPixel(x, y).R;
					byte g = bitmap1.GetPixel(x, y).G;
					byte b = bitmap1.GetPixel(x, y).B;
					y++;
					if (y != 0 && y % bitmap1.Height == 0)
						break;
					byte r1 = bitmap1.GetPixel(x, y).R;
					byte g1 = bitmap1.GetPixel(x, y).G;
					byte b1 = bitmap1.GetPixel(x, y).B;
					y++;
					if (y != 0 && y % bitmap1.Height == 0)
						break;
					byte r2 = bitmap1.GetPixel(x, y).R;
					byte g2 = bitmap1.GetPixel(x, y).G;
					byte b2 = bitmap1.GetPixel(x, y).B;

					renkler.AddRange(new byte[] { r, g, b, r1, g1, b1, r2, g2, b2 });
					if (Convert.ToInt32(renkler.Last()) == 254)
					{
						IsFinished = true;
						break;
					}
				}
			}
			bitmap1.Dispose();
			return renkler;
		}

		private string ConvertTurkishCharsToEnglish(string input)
		{
			//var text = "ÜST";
			var unaccentedText = String.Join("", input.Normalize(NormalizationForm.FormD)
					.Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark))
				.Replace("ı", "i")
				.Replace("İ", "I")
				.Replace("Ö", "O")
				.Replace("ö", "o")
				.Replace("Ü", "U")
				.Replace("ü", "u")
				.Replace("Ğ", "G")
				.Replace("g", "g")
				.Replace("Ş", "S")
				.Replace("ş", "s")
				.Replace("Ç", "C")
				.Replace("ç", "c");

			return unaccentedText;
		}

		private static void OpenImage(string fileName)
		{
			string path = Directory.GetCurrentDirectory() + fileName;
			Process process = new();
			process.StartInfo.FileName = "cmd.exe";
			process.StartInfo.Arguments = "/C " + path;
			process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
			_ = process.Start();
			process.WaitForExit();
		}

		private void ImageToHex(string fullName, string OutFileName)
		{
			Image img = Image.FromFile(fullName);
			byte[] arr;
			using (MemoryStream ms = new())
			{
				img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
				arr = ms.ToArray();
			}
			File.WriteAllBytes(OutFileName, arr);
			MessageBox.Show("Çevrim tamamlandı");
			//string fullName = @"C:\Users\AKR3P\Desktop\diifferanceImageTest\daghan.jpg";
			//string OutFileName = @"C:\Users\AKR3P\Desktop\diifferanceImageTest\daghan.txt";
			//ImageToHex(fullName, OutFileName);
			//fullName = @"C:\Users\AKR3P\Desktop\diifferanceImageTest\Yeni_Bitmap.jpeg";
			//OutFileName = @"C:\Users\AKR3P\Desktop\diifferanceImageTest\Yeni_Bitmap.txt";
			//ImageToHex(fullName, OutFileName);
			//List<byte> byteList = ReaderImageByte(bitmap1);
			//bool IsNumberOne = false;
		}

		private void Btn_Select_Data_Text_Click(object sender, EventArgs e)
		{
			fileDialog = new OpenFileDialog();
			fileDialog.DefaultExt = "*.txt";
			fileDialog.Filter = "Tüm dosyalar (*.*)|*.*";

			if (fileDialog.ShowDialog() == DialogResult.OK)
			{
				FileStream fileStream = File.OpenRead(fileDialog.FileName);

				using (StreamReader file = new StreamReader(fileStream))
				{
					int counter = 0;
					string ln;
					while ((ln = file.ReadLine()) != null)
					{
						richTextBox1.Text += ln;
						counter++;
					}
					file.Close();
					richTextBox1.Text += $"File has {counter} lines.";
				}

			}
		}

		private void richTextBox1_TextChanged(object sender, EventArgs e)
		{

		}
	}
}