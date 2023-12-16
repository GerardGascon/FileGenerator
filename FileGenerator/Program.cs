using System;
using System.IO;
using FileGenerator;

class Program {
	static void Main() {
		//GenerateWithInputSize();
		GenerateWithRandomSize(50);
	}

	const int BufferSize = 1024;

	static void GenerateWithInputSize() {
		Console.Write("Enter file size in KB: ");
		string input = Console.ReadLine() ?? string.Empty;

		if (input == string.Empty) {
			Console.WriteLine("Invalid input!");
			return;
		}

		long fileSize = long.Parse(input) * BufferSize;

		Console.Write("Enter file name: ");
		input = Console.ReadLine() ?? string.Empty;

		if (input == string.Empty) {
			Console.WriteLine("Invalid input!");
			return;
		}

		CreateBinaryFile(input, fileSize);

		Console.WriteLine("Binary file created successfully!");
		Console.WriteLine($"File created at: {AppDomain.CurrentDomain.BaseDirectory}{input}");
	}
	
	static void GenerateWithRandomSize(int numberOfFiles) {
		Random rnd = new();
		for (int i = 0; i < numberOfFiles; i++) {
			int kb = rnd.Next(0, 2_000_000);
			long fileSize = kb * BufferSize;
			
			string fileName = $"{kb}KB";
			CreateBinaryFile(fileName, fileSize);
			
			Console.WriteLine("Binary file created successfully!");
			Console.WriteLine($"File created at: {AppDomain.CurrentDomain.BaseDirectory}{fileName}");
		}
	}

	static void CreateBinaryFile(string filePath, long fileSize) {
		byte[] buffer = new byte[BufferSize];

		using FileStream fileStream = new($"{AppDomain.CurrentDomain.BaseDirectory}{filePath}", FileMode.Create);
		long bytesWritten = 0;

		ProgressBar p = new();

		while (bytesWritten < fileSize) {
			int bytesToWrite = (int)Math.Min(BufferSize, fileSize - bytesWritten);
			new Random().NextBytes(buffer); // Generate random data

			fileStream.Write(buffer, 0, bytesToWrite);

			bytesWritten += bytesToWrite;

			// Print progress
			double progress = (double)bytesWritten / fileSize * 100;
			p.Report(progress / 100);
		}

		p.Dispose();
	}
}