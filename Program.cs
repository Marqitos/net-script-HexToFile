using System.Text;
using System.Text.RegularExpressions;

FileStreamOptions options = new FileStreamOptions() {
    Mode = FileMode.Open,
    Access = FileAccess.Read
};
string text;
using (StreamReader streamReader = new StreamReader("products.sql", Encoding.UTF8, true, options)) {
    text = streamReader.ReadToEnd();
}
Regex regex = new Regex(@"\('([0-9a-f-]+)'[^x)]*x([0-9A-F]{5,})", RegexOptions.Multiline);
MatchCollection matches = regex.Matches(text);
foreach (Match match in matches) {
    string fileName = match.Groups[1].Value;
    string hexText = match.Groups[2].Value;
    using FileStream fileStream = new FileStream($"{fileName}.png", FileMode.Create, FileAccess.Write);
    for (int i = 0; i < hexText.Length; i += 2) {
        byte bin = BitConverter.GetBytes(Convert.ToInt32(hexText.Substring(i, 2), 16))[0];
        fileStream.WriteByte(bin);
    }
    fileStream.Close();
}
