using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EllipticCurve;
using System.Security.Cryptography;

namespace Thin_Magician_Coin
{
    internal class Transaction
    {
        public PublicKey FromAddress {get;set;}
        public PublicKey ToAddress { get; set; }
        public Decimal Amount { get; set; }
        public Signature? Signature { get; set; }

        public Transaction(PublicKey fromAddress, PublicKey toAddress, decimal Amount)
        {
            this.ToAddress = toAddress;
            this.FromAddress = fromAddress;
            this.Amount = Amount;
        }

        public string CalculateHash()
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string toAddressDER = BitConverter.ToString(ToAddress.toDer()).Replace("-", "");
            string transactionData = fromAddressDER + toAddressDER + Amount;
            byte[] transactionDataBytes = Encoding.ASCII.GetBytes(transactionData);
            return BitConverter.ToString(SHA256.Create().ComputeHash(transactionDataBytes)).Replace("-", "");
        }

        public void SignTransaction(PrivateKey signingKey)
        {
            string fromAddressDER = BitConverter.ToString(FromAddress.toDer()).Replace("-", "");
            string signingDER = BitConverter.ToString(signingKey.publicKey().toDer()).Replace("-", "");

           if (fromAddressDER != signingDER)
            {
                throw new Exception("You cannot sign transactions for another wallet!");
            }

            string txHash = this.CalculateHash();
            this.Signature = Ecdsa.sign(txHash, signingKey);
        }

        public bool IsValid()
        {
            if (this.Signature is null) { return false; }
            return Ecdsa.verify(this.CalculateHash(), this.Signature, this.FromAddress);
        }
    }
}
