using System;
using System.Management;
using AccountingLib.Entities;
using DocMageFramework.AppUtils;


namespace AccountingLib.Security
{
    /// <summary>
    /// Classe que fornece métodos de proteção aos recursos de acesso ao sistema ( tokens e senhas )
    /// Disponível também no instalador ( duplicação proposital de linhas de código, não refatorar )
    /// </summary>
    public static class ResourceProtector
    {
        /// <summary>
        /// Protege o login recem criado atribuindo um password default
        /// </summary> 
        public static void RectifyPassword(Login login)
        {
            // Caso o password não esteja definido, coloca um valor inicial no atributo
            if (String.IsNullOrEmpty(login.password))
                login.password = Cipher.GenerateHash("data01");
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
