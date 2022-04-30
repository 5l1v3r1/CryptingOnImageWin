using System.Diagnostics;
using System.Drawing.Imaging;
using System.Globalization;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace CryptingOnImageWin
{
	public partial class Form1 : Form
	{
		OpenFileDialog fileDialog;

		public Form1()
		{
			InitializeComponent();
			RSAMain2(null);
			RSAMain(null);
		}
		private static string _privateKey = "<RSAKeyValue><Modulus>l/RNvhleFrLyGZmrlXS28FnyegiMWB/nx34Lk/uoyKyPKTs1n5dHqZvEf68cVPcPuunFfmkFEGHAEuIt0bgkQE0lE1uZTkt9krizaOrlz70itvzCQLQELx6AgcBrNoRzBJgb8iswXXcqZJGiv8vXcWZussITsIyWPF8nv9Wo7os=</Modulus><Exponent>AQAB</Exponent><P>+6bCNR6I7hf6dReEXlyP/jVlRrVC2JBwo4CpC23F26iQt6vgnw9DbCnU0JkMztp3vCPRpkcUYsSe1WVutR3KWQ==</P><Q>mpSCuloJhVV3Vdk/ZVKyI/oC8BntuHQXyrBEpsT6GUkY6ClR+fzY7qXm4dsNLefEk3jWDlZVZpLAoir9M0sbgw==</Q><DP>+QhakiSojy4NfciLBrKFEKUYlzOTdiriPPt6zkOQlMKojAq8W8vIBgcTgmajRSTvbrDnrKwUoXVAVZ9uQi+76Q==</DP><DQ>QywsDszXfFBidjTcvqwL62RWaH3r9o7EU/j7LDlGsTw5jZF/JRICNtpzFUx6Uqt3mdoVss9DdzpoaVAjDWoi/w==</DQ><InverseQ>1vQgbWQMszSPGnt4xOXZlhVLmdg5mAAMwPQb2L6o+NhYbHNDBBdpUiYxgjr5HsU8PtQNaj1JZfvcaNf8B7pFgQ==</InverseQ><D>NwLc+ktYwsL53sbBVZQxfoYxwHhDxsuWL6S0MFjVXAEMuxKScTplWUPpOYh9q0zeRv7G3uLKUIny7WMwz1Ho3UN/FZoTp62q+SDabMvrdgx4tTX4rwjcoM4HoUQcsN93JyXnBwezVFcdgzF5Q9HajaO7WwD+ptOceIDnSh0EJ6E=</D></RSAKeyValue>";
		private static string _publicKey = "<RSAKeyValue><Modulus>l/RNvhleFrLyGZmrlXS28FnyegiMWB/nx34Lk/uoyKyPKTs1n5dHqZvEf68cVPcPuunFfmkFEGHAEuIt0bgkQE0lE1uZTkt9krizaOrlz70itvzCQLQELx6AgcBrNoRzBJgb8iswXXcqZJGiv8vXcWZussITsIyWPF8nv9Wo7os=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
		private static readonly UnicodeEncoding _encoder = new();
		//Bitmap myBitmap;
		//ImageCodecInfo myImageCodecInfo;
		//System.Drawing.Imaging.Encoder myEncoder;
		//EncoderParameter myEncoderParameter;
		//EncoderParameters myEncoderParameters; // Encrypt(richTextBox0.Text, publicKeyServer);

		//private static void RSA()
		//{
		//	var rsa = new RSACryptoServiceProvider();
		//	_privateKey = rsa.ToXmlString(true);
		//	_publicKey = rsa.ToXmlString(false);

		//	var text = "Test1";
		//	Console.WriteLine("RSA // Text to encrypt: " + text);
		//	var enc = Encrypt(text);
		//	Console.WriteLine("RSA // Encrypted Text: " + enc);
		//	var dec = Decrypt(enc);
		//	Console.WriteLine("RSA // Decrypted Text: " + dec);
		//}

		//public static string Decrypt(string data)
		//{
		//	var rsa = new RSACryptoServiceProvider();
		//	var dataArray = data.Split(new char[] { ',' });
		//	byte[] dataByte = new byte[dataArray.Length];
		//	for (int i = 0; i < dataArray.Length; i++)
		//	{
		//		dataByte[i] = Convert.ToByte(dataArray[i]);
		//	}

		//	rsa.FromXmlString(_privateKey);
		//	var decryptedByte = rsa.Decrypt(dataByte, false);
		//	return _encoder.GetString(decryptedByte);
		//}

		//public static string Encrypt(string data)
		//{
		//	var rsa = new RSACryptoServiceProvider();
		//	rsa.FromXmlString(_publicKey);
		//	var dataToEncrypt = _encoder.GetBytes(data);
		//	var encryptedByteArray = rsa.Encrypt(dataToEncrypt, false).ToArray();
		//	var length = encryptedByteArray.Count();
		//	var item = 0;
		//	var sb = new StringBuilder();
		//	foreach (var x in encryptedByteArray)
		//	{
		//		item++;
		//		sb.Append(x);
		//		if (item < length)
		//			sb.Append(',');
		//	}

		//	return sb.ToString();
		//}

		static void RSAMain(string[] args)
		{
			// The URI to sign.
			string resourceToSign = "http://www.microsoft.com";

			// The name of the file to which to save the XML signature.
			string XmlFileName = "xmldsig.xml";

			try
			{

				// Generate a signing key.
				RSA Key = RSA.Create();

				Console.WriteLine("Signing: {0}", resourceToSign);

				// Sign the detached resourceand save the signature in an XML file.
				SignDetachedResource(resourceToSign, XmlFileName, Key);

				Console.WriteLine("XML signature was successfully computed and saved to {0}.", XmlFileName);

				// Verify the signature of the signed XML.
				Console.WriteLine("Verifying signature...");

				//Verify the XML signature in the XML file.
				bool result = VerifyDetachedSignature(XmlFileName);

				// Display the results of the signature verification to 
				// the console.
				if (result)
				{
					Console.WriteLine("The XML signature is valid.");
				}
				else
				{
					Console.WriteLine("The XML signature is not valid.");
				}
			}
			catch (CryptographicException e)
			{
				Console.WriteLine(e.Message);
			}
		}

		// Sign an XML file and save the signature in a new file.
		public static void SignDetachedResource(string URIString, string XmlSigFileName, RSA Key)
		{
			// Create a SignedXml object.
			SignedXml signedXml = new();

			// Assign the key to the SignedXml object.
			signedXml.SigningKey = Key;

			// Create a reference to be signed.
			Reference reference = new();

			// Add the passed URI to the reference object.
			reference.Uri = URIString;

			// Add the reference to the SignedXml object.
			signedXml.AddReference(reference);

			// Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate).
			KeyInfo keyInfo = new();
			keyInfo.AddClause(new RSAKeyValue((RSA)Key));
			signedXml.KeyInfo = keyInfo;

			// Compute the signature.
			signedXml.ComputeSignature();

			// Get the XML representation of the signature and save
			// it to an XmlElement object.
			XmlElement xmlDigitalSignature = signedXml.GetXml();

			// Save the signed XML document to a file specified
			// using the passed string.
			XmlTextWriter xmltw = new(XmlSigFileName, new UTF8Encoding(false));
			xmlDigitalSignature.WriteTo(xmltw);
			xmltw.Close();
		}
		// Verify the signature of an XML file and return the result.
		public static Boolean VerifyDetachedSignature(string XmlSigFileName)
		{
			// Create a new XML document.
			XmlDocument xmlDocument = new XmlDocument();

			// Load the passed XML file into the document.
			xmlDocument.Load(XmlSigFileName);

			// Create a new SignedXMl object.
			SignedXml signedXml = new SignedXml();

			// Find the "Signature" node and create a new
			// XmlNodeList object.
			XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

			// Load the signature node.
			signedXml.LoadXml((XmlElement)nodeList[0]);

			// Check the signature and return the result.
			return signedXml.CheckSignature();
		}

		public static void RSAMain2(String[] args)
		{
			try
			{
				// Generate a signing key.
				RSA Key = RSA.Create();

				// Create an XML file to sign.
				CreateSomeXml("Example.xml");
				Console.WriteLine("New XML file created.");

				// Sign the XML that was just created and save it in a 
				// new file.
				SignXmlFile("Example.xml", "SignedExample.xml", Key);
				Console.WriteLine("XML file signed.");

				// Verify the signature of the signed XML.
				Console.WriteLine("Verifying signature...");
				bool result = VerifyXmlFile("SignedExample.xml");

				// Display the results of the signature verification to \
				// the console.
				if (result)
				{
					Console.WriteLine("The XML signature is valid.");
				}
				else
				{
					Console.WriteLine("The XML signature is not valid.");
				}
			}
			catch (CryptographicException e)
			{
				Console.WriteLine(e.Message);
			}
		}

		// Sign an XML file and save the signature in a new file.
		public static void SignXmlFile(string FileName, string SignedFileName, RSA Key)
		{
			// Create a new XML document.
			XmlDocument doc = new XmlDocument();

			// Format the document to ignore white spaces.
			doc.PreserveWhitespace = false;

			// Load the passed XML file using it's name.
			doc.Load(new XmlTextReader(FileName));

			// Create a SignedXml object.
			SignedXml signedXml = new SignedXml(doc);

			// Add the key to the SignedXml document. 
			signedXml.SigningKey = Key;

			// Create a reference to be signed.
			Reference reference = new Reference();
			reference.Uri = "";

			// Add an enveloped transformation to the reference.
			XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
			reference.AddTransform(env);

			// Add the reference to the SignedXml object.
			signedXml.AddReference(reference);

			// Add an RSAKeyValue KeyInfo (optional; helps recipient find key to validate).
			KeyInfo keyInfo = new KeyInfo();
			keyInfo.AddClause(new RSAKeyValue((RSA)Key));
			signedXml.KeyInfo = keyInfo;

			// Compute the signature.
			signedXml.ComputeSignature();

			// Get the XML representation of the signature and save
			// it to an XmlElement object.
			XmlElement xmlDigitalSignature = signedXml.GetXml();

			// Append the element to the XML document.
			doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

			if (doc.FirstChild is XmlDeclaration)
			{
				doc.RemoveChild(doc.FirstChild);
			}

			// Save the signed XML document to a file specified
			// using the passed string.
			XmlTextWriter xmltw = new XmlTextWriter(SignedFileName, new UTF8Encoding(false));
			doc.WriteTo(xmltw);
			xmltw.Close();
		}
		// Verify the signature of an XML file and return the result.
		public static Boolean VerifyXmlFile(String Name)
		{
			// Create a new XML document.
			XmlDocument xmlDocument = new XmlDocument();

			// Format using white spaces.
			xmlDocument.PreserveWhitespace = true;

			// Load the passed XML file into the document. 
			xmlDocument.Load(Name);

			// Create a new SignedXml object and pass it
			// the XML document class.
			SignedXml signedXml = new SignedXml(xmlDocument);

			// Find the "Signature" node and create a new
			// XmlNodeList object.
			XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

			// Load the signature node.
			signedXml.LoadXml((XmlElement)nodeList[0]);

			// Check the signature and return the result.
			return signedXml.CheckSignature();
		}

		// Create example data to sign.
		public static void CreateSomeXml(string FileName)
		{
			// Create a new XmlDocument object.
			XmlDocument document = new XmlDocument();

			// Create a new XmlNode object.
			XmlNode node = document.CreateNode(XmlNodeType.Element, "", "MyElement", "samples");

			// Add some text to the node.
			node.InnerText = "Example text to be signed.";

			// Append the node to the document.
			document.AppendChild(node);

			// Save the XML document to the file name specified.
			XmlTextWriter xmltw = new XmlTextWriter(FileName, new UTF8Encoding(false));
			document.WriteTo(xmltw);
			xmltw.Close();
		}

		public byte[] StartRSAEncrypt(string text)
		{
			try
			{
				// Bayt dizisi ve dize arasında dönüştürmek için bir UnicodeEncoder oluşturun.
				UnicodeEncoding ByteConverter = new();

				//Orijinal, şifrelenmiş ve şifresi çözülmüş verileri tutmak için bayt dizileri oluşturun.
				byte[] dataToEncrypt = ByteConverter.GetBytes(text);
				byte[] encryptedData;
				//byte[] decryptedData;

				// Oluşturmak için yeni bir RSACryptoServiceProvider örneği oluşturun
				//genel ve özel anahtar verileri.
				using (RSACryptoServiceProvider RSA = new(1024))
				{

					//Verileri, genel anahtar bilgisi olan ENCRYPT'e iletin
					//(RSACryptoServiceProvider.ExportParameters(false) kullanılarak),
					// ve OAEP dolgusu olmadığını belirten bir boole bayrağı.
					encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

					//Verileri DECRYPT'e iletin, özel anahtar bilgisi
					//(RSACryptoServiceProvider.ExportParameters(true) kullanılarak),
					// ve OAEP dolgusu olmadığını belirten bir boole bayrağı.
					//decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

					//Şifrelenmiş düz metni konsola göster.
					//richTextBox1.Text += "Decrypted plaintext: " + ByteConverter.GetString(decryptedData);
				}
				return encryptedData;
			}
			catch (ArgumentNullException)
			{
				//Şifrelemenin yapması durumunda bu istisnayı yakala
				//başarısız.
				Console.WriteLine("Encryption failed.");
			}
			return null;
		}

		public byte[] StartRSARSADecrypt(byte[] encryptedData)
		{
			try
			{
				// Bayt dizisi ve dize arasında dönüştürmek için bir UnicodeEncoder oluşturun.
				UnicodeEncoding ByteConverter = new();

				//Orijinal, şifrelenmiş ve şifresi çözülmüş verileri tutmak için bayt dizileri oluşturun.
				//byte[] dataToEncrypt = ByteConverter.GetBytes(text);
				//byte[] encryptedData;
				byte[] decryptedData;

				// Oluşturmak için yeni bir RSACryptoServiceProvider örneği oluşturun
				//genel ve özel anahtar verileri.
				using (RSACryptoServiceProvider RSA = new(1024))
				{

					//Verileri, genel anahtar bilgisi olan ENCRYPT'e iletin
					//(RSACryptoServiceProvider.ExportParameters(false) kullanılarak),
					// ve OAEP dolgusu olmadığını belirten bir boole bayrağı.
					//encryptedData = RSAEncrypt(dataToEncrypt, RSA.ExportParameters(false), false);

					//Verileri DECRYPT'e iletin, özel anahtar bilgisi
					//(RSACryptoServiceProvider.ExportParameters(true) kullanılarak),
					// ve OAEP dolgusu olmadığını belirten bir boole bayrağı.
					decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);

					//Şifrelenmiş düz metni konsola göster.
					//richTextBox1.Text += "Decrypted plaintext: " + ByteConverter.GetString(decryptedData);
				}
				return decryptedData;
			}
			catch (ArgumentNullException)
			{
				//Şifrelemenin yapması durumunda bu istisnayı yakala
				//başarısız.
				Console.WriteLine("Encryption failed.");
			}
			return null;
		}

		public static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
		{
			try
			{
				byte[] encryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new(1024))
				{

					//Import the RSA Key information. This only needs
					//toinclude the public key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Encrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
				}
				return encryptedData;
			}
			//Catch and display a CryptographicException  
			//to the console.
			catch (CryptographicException e)
			{
				Console.WriteLine(e.Message);

				return null;
			}
		}

		public static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
		{
			try
			{
				byte[] decryptedData;
				//Create a new instance of RSACryptoServiceProvider.
				using (RSACryptoServiceProvider RSA = new(1024))
				{
					//Import the RSA Key information. This needs
					//to include the private key information.
					RSA.ImportParameters(RSAKeyInfo);

					//Decrypt the passed byte array and specify OAEP padding.  
					//OAEP padding is only available on Microsoft Windows XP or
					//later.  
					decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
				}
				return decryptedData;
			}
			//Catch and display a CryptographicException  
			//to the console.
			catch (CryptographicException e)
			{
				Console.WriteLine(e.ToString());

				return null;
			}
		}

		public static string Encrypt(string Text, string publicKey)
		{
			var testData = Encoding.UTF8.GetBytes(Text);
			using (var rsa = new RSACryptoServiceProvider(1024))
			{
				try
				{
					rsa.FromXmlString(publicKey.ToString());
					var encryptedData = rsa.Encrypt(testData, true);
					var base64Encrypted = Convert.ToBase64String(encryptedData);
					return base64Encrypted;
				}
				finally
				{
					rsa.PersistKeyInCsp = false;
				}
			}
		}

		public static string Decrypt(string Text)
		{
			string BOS = "";
			try
			{
				var privateKey = _privateKey;
				var testData = Encoding.UTF8.GetBytes(Text);
				using (var rsa = new RSACryptoServiceProvider(1024))
				{
					try
					{
						var base64Encrypted = Text;
						rsa.FromXmlString(privateKey);
						var resultBytes = Convert.FromBase64String(base64Encrypted);
						var decryptedBytes = rsa.Decrypt(resultBytes, true);
						var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
						return decryptedData.ToString();
					}
					finally
					{
						rsa.PersistKeyInCsp = false;
					}
				}
			}
			catch { return BOS; }
		}


		private void Btn_ResimSec_Click(object sender, EventArgs e)
		{
			openFileDialog1.DefaultExt = "*.*";
			openFileDialog1.Filter = "Tüm dosyalar (*.*)|*.*";

			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				Bitmap bmp = new(openFileDialog1.FileName);
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

		private string EncryptText(string text)
		{
			int mod = text.Length % 82;
			int byteCount = text.Length / 82;
			byteCount += mod != 0 ? 1 : 0;
			string s = "";
			for (int i = 0; i < text.Length; i += byteCount)
			{
				s += Encrypt(text.Substring(i, byteCount), _publicKey);
			}
			return s;
		}

		private string DecryptText(string text)
		{
			string s = "";
			for (int i = 0; i < text.Length; i += 172)
			{
				s += Decrypt(text.Substring(i, 172));
			}
			return s;
		}

		private void WriteDataOnImage(Bitmap bitmap1, string dataStr)
		{
			dataStr = ConvertTurkishCharsToEnglish(dataStr);

			string encryptText = EncryptText(dataStr);// 172

			richTextBox3.Text = encryptText;
			char[] chars = dataStr.ToCharArray();

			Rectangle rect = new(0, 0, bitmap1.Width, bitmap1.Height);
			BitmapData bmpData =
				bitmap1.LockBits(rect, ImageLockMode.ReadWrite,
				bitmap1.PixelFormat);

			IntPtr ptr = bmpData.Scan0;

			int bytes = Math.Abs(bmpData.Stride) * bitmap1.Height;
			// bytes >= chars olmalı
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
					if (chrShrt == 126 || chars[c] == '~')
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

			MemoryStream ms = new();
			bitmap1.Save(ms, ImageFormat.Png);
			using (FileStream file = new(Txt_Name.Text + ".png", FileMode.Create, FileAccess.Write))
			{
				ms.WriteTo(file);
				ms.Close();
			}

			//richTextBox1.Text += "\n----------------------------\n";
			bitmap1.Dispose();
			MessageBox.Show("Tebrikler! İşlem başarılı", "Başarılı", MessageBoxButtons.OK);
		}

		private void ReadDataInImage(Bitmap bitmap1)
		{
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
			string cryptText = "";
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
					cryptText += Convert.ToChar(bnryByte);
					if (bnryByte == 126 || bnryByte == '~')
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
			richTextBox2.Text += cryptText;
			#region Decrypt
			List<char> chars = new();
			bool IsLast = false;
			for (int i = 0; i < richTextBox3.Text.Length; i += 172)
			{
				string cozum = DecryptText(richTextBox3.Text.Substring(i, 172));
				foreach (var ch in cozum)
				{
					chars.Add(ch);
					richTextBox4.Text += ch;
					if (ch == '~' || ch == 126)
					{
						IsLast = true;
						break;
					}
				}
				if (IsLast)
				{
					break;
				}
			}
			#endregion
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