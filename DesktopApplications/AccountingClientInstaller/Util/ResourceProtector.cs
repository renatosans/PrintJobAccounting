using System;
using System.Text;
using System.Management;
using System.Security.Cryptography;


namespace AccountingClientInstaller.Util
{
    /// <summary>
    /// Classe que fornece métodos de proteção aos recursos de acesso ao sistema ( tokens e senhas )
    /// Disponível também no sistema ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class ResourceProtector
    {
        private const String salt = "EJ%S$@!";

        /// <summary>
        /// Gera um hash MD5 para proteção da senha ou token
        /// </summary>
        public static String GenerateHash(String inputString)
        {
            byte[] inputBuffer = Encoding.UTF8.GetBytes(inputString + salt);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] outputBuffer = md5.ComputeHash(inputBuffer);
            StringBuilder hash = new StringBuilder();
            foreach (Byte outputByte in outputBuffer)
            {
                hash.Append(outputByte.ToString("x2").ToUpper());
            }

            return hash.ToString();
        }

        /// <summary>
        /// Obtem uma identificação única do hardware onde o sistema está em execução
        /// </summary>
        public static String GetHardwareId()
        {
            String hardwareId = null;

            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"C:\"");
            disk.Get();
            String hardDiskId = disk["VolumeSerialNumber"].ToString();

            String processorId = null;
            ManagementClass objectType = new ManagementClass("Win32_Processor");
            ManagementObjectCollection availableProcessors = objectType.GetInstances();
            foreach (ManagementObject processor in availableProcessors)
            {
                if (String.IsNullOrEmpty(processorId))
                    processorId = processor.Properties["ProcessorId"].Value.ToString();
            }

            hardwareId = processorId + "_" + hardDiskId;
            return hardwareId;
        }
    }

}
