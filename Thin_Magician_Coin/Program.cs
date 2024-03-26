using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using EllipticCurve;
using Newtonsoft.Json;

namespace Thin_Magician_Coin
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PrivateKey key1 = new PrivateKey();
            PublicKey wallet1 = key1.publicKey();

            PrivateKey key2 = new PrivateKey();
            PublicKey wallet2 = key2.publicKey();

            PrivateKey key3 = new PrivateKey();
            PublicKey wallet3 = key3.publicKey();

            Blockchain thinMagicianCoin = new Blockchain(2, 100);

            thinMagicianCoin.MinePendingTransactions(wallet1);
            thinMagicianCoin.MinePendingTransactions(wallet2);
            thinMagicianCoin.MinePendingTransactions(wallet3);

            Console.Write("\nBalance of Wallet1: $" + thinMagicianCoin.GetBalanceOfWallet(wallet1).ToString());
            Console.Write("\nBalance of Wallet2: $" + thinMagicianCoin.GetBalanceOfWallet(wallet2).ToString());
            Console.Write("\nBalance of Wallet3: $" + thinMagicianCoin.GetBalanceOfWallet(wallet3).ToString());

            Transaction tx1 = new Transaction(wallet1, wallet2, 55.00m);
            tx1.SignTransaction(key1);
            thinMagicianCoin.addPendingTransaction(tx1);

            Transaction tx2 = new Transaction(wallet3, wallet2, 20.00m);
            tx1.SignTransaction(key3);
            thinMagicianCoin.addPendingTransaction(tx2);

            thinMagicianCoin.MinePendingTransactions(wallet3);

            string blockJSON = JsonConvert.SerializeObject(thinMagicianCoin, Formatting.Indented);

            Console.WriteLine(blockJSON);

            if (thinMagicianCoin.IsChainValid())
            {
                Console.WriteLine("Blockchain is valid");
            }
            else
            {
                Console.WriteLine("Blockchain is not valid");
            }
        }
    }
}
