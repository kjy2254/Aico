using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Aico
{
    internal class FileController
    {
        static void Main(String[] args)
        {
            string path = @"C:\persist-root";
            //Console.WriteLine(path);
            string textValue = System.IO.File.ReadAllText(path).Replace(@"\", "");
            textValue = textValue.Replace("{", "{\n");
            textValue = textValue.Replace("}", "\n}");

            int a = textValue.IndexOf("transactions") - 1;
            int b = textValue.IndexOf("internalTransactions");

            textValue = textValue.Substring(a, b-a-2);
            textValue = textValue.Replace(",", "\n");
            textValue = textValue.Replace("{", "\n{");
            textValue = textValue.Replace("\"", "");
            textValue = textValue.Replace("\"", "");
            textValue = textValue.Replace("insertImportTime:false", "");
            textValue = textValue.Replace("insertImportTime:true", "");

            string[] separatedText = textValue.Split("toSmartContract:");

            Dictionary<string, string>[] matrix = new Dictionary<string, string>[separatedText.Length-1];

            for(int i = 0; i < separatedText.Length-1; i++)
            {
                matrix[i] = new Dictionary<string, string>();
            }
 
            for (int i = 0; i < separatedText.Length-1; i++)
            {
                string[] separatedByLine = separatedText[i].Split("\n");
                for(int j = 0; j < separatedByLine.Length; j++)
                {
                    string temp = separatedByLine[j].Substring(0, separatedByLine[j].IndexOf(':')+1);
                    switch (temp){
                        case "id:":
                            //matrix[i].Add("id", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':')+1));
                            break;
                        case "blockNumber:":
                            //matrix[i].Add("blockNumber", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "networkID:":
                            //matrix[i].Add("networkID", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "chainId:":
                            //matrix[i].Add("chainId", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "status:":
                            //matrix[i].Add("status", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "time:":
                            matrix[i].Add("time", TimeStampToDateTime(separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1)));
                            break;
                        case "from:":
                            matrix[i].Add("from", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "gas:":
                            //matrix[i].Add("gas", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "gasPrice:":
                            //matrix[i].Add("gasPrice", hexToEther(separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1)));
                            break;
                        case "gasUsed:":
                            //matrix[i].Add("gasUsed", hexToEther(separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1)));
                            break;
                        case "nonce:":
                            //matrix[i].Add("nonce", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "to:":
                            matrix[i].Add("to", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "value:":
                            matrix[i].Add("value", hexToEther(separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1)));
                            break;
                        case "maxFeePerGas:":
                            //matrix[i].Add("maxFeePerGas", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "maxPriorityFeePerGas:":
                            //matrix[i].Add("maxPriorityFeePerGas", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "estimatedBaseFee:":
                            //matrix[i].Add("estimatedBaseFee", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "verifiedOnBlockchain:":
                            //matrix[i].Add("verifiedOnBlockchain", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "rawTransaction:":
                            //matrix[i].Add("rawTransaction", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                        case "transactionHash:":
                            matrix[i].Add("transactionHash", separatedByLine[j].Substring(separatedByLine[j].IndexOf(':') + 1));
                            break;
                    }
                }
            }

            string output = "";
            int transactionNumber = 1;
            for(int i = matrix.Length-1; i >= 0; i--)
            {
                output += "transaction #" + transactionNumber + "\n";
                transactionNumber++;
                //Console.WriteLine("\ntransaction #{0}", i+1);
                foreach (var pair in matrix[i])
                {
                    //Console.WriteLine("{0}:{1}", pair.Key, pair.Value);
                    output += pair.Key + ": " + pair.Value + "\n";
                }
                output += "\n";
            }
            Console.WriteLine(output);

            string savePath = @"C:\test\output.txt";
            File.WriteAllText(savePath, output);
        }
        static string hexToEther(string hex)
        {
            double mod = 1000000000000000000;
            double dec = Convert.ToInt64(hex, 16)/mod;
            string ether = dec.ToString("0.###############") + " Ether";
            //Console.WriteLine(ether);
            return ether;
        }
        static string TimeStampToDateTime(string timeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(Convert.ToInt64(timeStamp)).ToLocalTime();
            string date = dt.ToString();
            //Console.WriteLine(date);
            return date;
        }
    }
}
