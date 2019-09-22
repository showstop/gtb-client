using System;
using System.IO;
using System.Text;

    /// <summary>
    /// AES : 128 bit Rijndael
    /// </summary>
    public class AES
    {
        private byte[] key_;
        private byte[] iv_;

        public AES(byte[] aKey, byte[] aIV)
        {
            if(aKey == null)    
                throw new ArgumentException("AES Key cannot be null");
            if(aIV == null)
                throw new ArgumentException("AES IV cannot be null");

            this.key_ = aKey;
            this.iv_ = aIV;
        }

        public byte[] encryptBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
                return data;

            for (int i = 0, k = 0; i < data.Length; i++, k++)
            {
                data[i] ^= key_[k];
                data[i] ^= iv_[k];

                if (k == key_.Length - 1)
                    k = 0;
            }
            return data;
        }

		public byte[] decryptBytes(byte[] data)
        {
            return encryptBytes(data);
        }

    }
