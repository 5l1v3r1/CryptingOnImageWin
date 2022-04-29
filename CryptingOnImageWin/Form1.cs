using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

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
		Bitmap myBitmap;
		ImageCodecInfo myImageCodecInfo;
		System.Drawing.Imaging.Encoder myEncoder;
		EncoderParameter myEncoderParameter;
		EncoderParameters myEncoderParameters;

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
				WriteDataOnImage(bmp, richTextBox1.Text);
			}
		}

		private void Btn_ResimOku_Click(object sender, EventArgs e)
		{
			openFileDialog1.DefaultExt = "*.*";
			openFileDialog1.Filter = "Tüm dosyalar (*.*)|*.*";

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				//FileStream fileStream = new FileStream(openFileDialog1.FileName,FileMode.Open);

				Bitmap bmp = new Bitmap(openFileDialog1.FileName);
				//Task.Run(() => ReadImage(bmp));
				ReadDataInImage(bmp);
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

		private byte[] BinaryToByteArray(string stringBinary)
		{
			int numOfBytes = stringBinary.Length / 8;
			byte[] bytes = new byte[numOfBytes];
			for (int i = 0; i < numOfBytes; ++i)
			{
				bytes[i] = Convert.ToByte(stringBinary.Substring(8 * i, 8), 2);
			}
			return bytes;
		}

		private short[] BinaryToShorts(string stringBinary)
		{
			int numOfShorts = stringBinary.Length / 16;
			short[] shorts = new short[numOfShorts];
			for (int i = 0; i < numOfShorts; ++i)
			{
				shorts[i] = Convert.ToInt16(stringBinary.Substring(16 * i, 16), 2);
			}
			return shorts;
		}

		private int BinaryToInt(string stringBinary)
		{
			return Convert.ToInt32(stringBinary, 2);
		}

		private byte BinaryToByte(string stringBinary)
		{
			return Convert.ToByte(stringBinary, 2);
		}

		private ushort BinaryToUshort(string stringBinary)
		{
			return Convert.ToUInt16(stringBinary, 2);
		}

		private byte[] CharToBytes(char ch)
		{
			return BitConverter.GetBytes(ch);
		}

		private short CharToShort(char ch)
		{
			return Convert.ToInt16(ch);
		}

		public static string ToBinary(byte x)
		{
			char[] buff = new char[8];

			for (int i = 7; i >= 0; i--)
			{
				int mask = 1 << i;
				buff[7 - i] = (x & mask) != 0 ? '1' : '0';
			}

			return new string(buff);
		}

		public static string ToBinary(ushort x)
		{
			char[] buff = new char[16];

			for (int i = 15; i >= 0; i--)
			{
				int mask = 1 << i;
				buff[15 - i] = (x & mask) != 0 ? '1' : '0';
			}

			return new string(buff);
		}

		public static string ToBinary(int x)
		{
			char[] buff = new char[32];

			for (int i = 31; i >= 0; i--)
			{
				int mask = 1 << i;
				buff[31 - i] = (x & mask) != 0 ? '1' : '0';
			}

			return new string(buff);
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
								byte yeniBytes = BinaryToByteArray(yenidata).First();
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

		private void WriteDataOnImage(Bitmap bitmap1, string dataStr)
		{
			dataStr = ConvertTurkishCharsToEnglish(dataStr);
			char[] chars = dataStr.ToCharArray();

			Rectangle rect = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);
			BitmapData bmpData =
				bitmap1.LockBits(rect, ImageLockMode.ReadWrite,
				bitmap1.PixelFormat);

			IntPtr ptr = bmpData.Scan0;

			int bytes = Math.Abs(bmpData.Stride) * bitmap1.Height;
			byte[] rgbValues = new byte[bytes];

			System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

			string charbnry = ToBinary(chars.Length);
			string s = "";
			for (int i = 0; i < charbnry.Length; i++)
			{
				char c = charbnry[i];
				s += c;
			}
			byte[] bytDizi = BinaryToByteArray(s);
			rgbValues[56] = bytDizi[0];
			rgbValues[57] = bytDizi[1];
			rgbValues[58] = bytDizi[2];
			rgbValues[59] = bytDizi[3];

			bool IsLastChar = false;
			for (int j = 64; j < rgbValues.Length; j++)
			{
				for (int c = 0; c < chars.Length; c++)
				{
					byte chrShrt = Convert.ToByte(chars[c]);

					string bnry = ToBinary(chrShrt);
					for (int r = 0; r < 8; r++)
					{
						string rgbStr = ToBinary(rgbValues[j]).Substring(0, 7);
						rgbValues[j] = BinaryToByte(rgbStr + bnry[r]);
						j++;
					}
					if (chrShrt == 43)
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

			System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);

			bitmap1.UnlockBits(bmpData);

			PaintEventArgs e = new(Graphics.FromImage(bitmap1), rect);
			e.Graphics.DrawImage(bitmap1, bitmap1.Width, bitmap1.Height);

			MemoryStream ms = new MemoryStream();
			bitmap1.Save(ms, ImageFormat.Png);
			using (FileStream file = new FileStream(Txt_Name.Text + ".png", FileMode.Create, System.IO.FileAccess.Write))
			{
				ms.WriteTo(file);
				ms.Close();
			}

			bitmap1.Dispose();
			MessageBox.Show("Tebrikler! İşlem başarılı", "Başarılı", MessageBoxButtons.OK);
		}

		private void ReadDataInImage(Bitmap bitmap1)
		{
			richTextBox1.Text += "\n----------------------------\n";

			Rectangle rect = new Rectangle(0, 0, bitmap1.Width, bitmap1.Height);
			var bmpData =
				bitmap1.LockBits(rect, ImageLockMode.ReadWrite,
				bitmap1.PixelFormat);

			IntPtr ptr = bmpData.Scan0;

			int bytes = Math.Abs(bmpData.Stride) * bitmap1.Height;

			byte[] rgbValues = new byte[bytes];

			System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

			string ss = ToBinary(rgbValues[56]) + ToBinary(rgbValues[57]) + ToBinary(rgbValues[58]) + ToBinary(rgbValues[59]);
			int nt = BinaryToInt(ss);

			bool IsLastChar = false;
			for (int j = 64; j < rgbValues.Length; j++)
			{
				for (int c = 0; c < nt; c++)
				{
					string s = "";
					for (int i = 0; i < 8; i++)
					{
						s += ToBinary(rgbValues[j]).Last();
						j++;
					}
					byte bnryByte = BinaryToByte(s);
					richTextBox1.Text += Convert.ToChar(bnryByte);
					if (bnryByte == 43)
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
			bitmap1.Dispose();
			MessageBox.Show("Tebrikler! İşlem başarılı", "Başarılı", MessageBoxButtons.OK);
		}

		private static ImageCodecInfo GetEncoderInfo(String mimeType)
		{
			int j;
			ImageCodecInfo[] encoders;
			encoders = ImageCodecInfo.GetImageEncoders();
			for (j = 0; j < encoders.Length; ++j)
			{
				if (encoders[j].MimeType == mimeType)
					return encoders[j];
			}
			return null;
		}

		private void SetPixel_Example(PaintEventArgs e, Bitmap myBitmap)
		{

			// Create a Bitmap object from a file.
			//Bitmap myBitmap = new Bitmap("Grapes.jpg");

			// Draw myBitmap to the screen.
			e.Graphics.DrawImage(myBitmap, 0, 0, myBitmap.Width,
				myBitmap.Height);

			// Set each pixel in myBitmap to black.
			for (int Xcount = 0; Xcount < myBitmap.Width; Xcount++)
			{
				for (int Ycount = 0; Ycount < myBitmap.Height; Ycount++)
				{
					myBitmap.SetPixel(Xcount, Ycount, Color.Black);
				}
			}

			// Draw myBitmap to the screen again.
			e.Graphics.DrawImage(myBitmap, myBitmap.Width, 0,
				myBitmap.Width, myBitmap.Height);
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