using System;
using System.Net;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Collections.Generic;
using AccountingLib.Entities;
using DocMageFramework.AppUtils;
using DocMageFramework.JobExecution;
using DocMageFramework.DataManipulation;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;


namespace PrintLogRouter
{
    public class DeviceDiscoverer : IPeriodicTask, IListener
    {
        private NameValueCollection taskParams;

        private Discoverer discoverer;

        private List<PrintingDevice> deviceList;

        private String serviceUrl;

        private int tenantId;


        public DeviceDiscoverer()
        {
            this.discoverer = new Discoverer();
            this.deviceList = new List<PrintingDevice>();
        }

        public void InitializeTaskState(NameValueCollection taskParams, DataAccess dataAccess)
        {
            this.taskParams = taskParams;
        }

        public void NotifyObject(Object obj)
        {
            if (obj is String)
            {
                String errorMessage = (String)obj;
                if (EventLog.SourceExists("Print Log Router")) EventLog.WriteEntry("Print Log Router", errorMessage);
            }
        }

        // Serial BROTHER - .1.3.6.1.2.1.43.5.1.1.17.1
        // Serial RICOH - .1.3.6.1.4.1.367.3.2.1.2.1.4.0
        // Serial LEXMARK - .1.3.6.1.4.1.641.2.1.2.1.6.1
        // Page Count - .1.3.6.1.2.1.43.10.2.1.4.1.1
        // Printer Description - .1.3.6.1.2.1.25.3.2.1.3.1
        // Printer Status - .1.3.6.1.2.1.25.3.5.1.1.1
        // Toner Estimated Capacity % - .1.3.6.1.4.1.367.3.2.1.2.24.1.1.5.1
        private String GetDeviceDescription(IPAddress printingDeviceIP)
        {
            String description = null;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(printingDeviceIP, 161);
                OctetString octetString = new OctetString("public");
                List<Variable> inputVars = new List<Variable> { new Variable(new ObjectIdentifier(".1.3.6.1.2.1.25.3.2.1.3.1")) };

                List<Variable> outputVars = (List<Variable>)Messenger.Get(VersionCode.V1, endPoint, octetString, inputVars, 60000);
                if (outputVars.Count > 0)
                    description = outputVars[0].Data.ToString();
            }
            catch (Exception exc)
            {
                NotifyObject(exc.Message + exc.StackTrace);
            }

            return description;
        }

        private String GetSerialNumber(IPAddress printingDeviceIP, String deviceDescription)
        {
            String serialNumber = null;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(printingDeviceIP, 161);
                OctetString octetString = new OctetString("public");
                String OID = ".1.3.6.1.2.1.47.1.1.1.1.11.1";
                if (deviceDescription.ToUpper().Contains("BROTHER")) OID = ".1.3.6.1.2.1.43.5.1.1.17.1";
                if (deviceDescription.ToUpper().Contains("RICOH")) OID = ".1.3.6.1.4.1.367.3.2.1.2.1.4.0";
                if (deviceDescription.ToUpper().Contains("LEXMARK")) OID = ".1.3.6.1.4.1.641.2.1.2.1.6.1";
                List<Variable> inputVars = new List<Variable> { new Variable(new ObjectIdentifier(OID)) };

                List<Variable> outputVars = (List<Variable>)Messenger.Get(VersionCode.V1, endPoint, octetString, inputVars, 60000);
                if (outputVars.Count > 0)
                    serialNumber = outputVars[0].Data.ToString();
            }
            catch (Exception exc)
            {
                NotifyObject(exc.Message + exc.StackTrace);
            }

            return serialNumber;
        }

        private String GetPageCounter(IPAddress printingDeviceIP)
        {
            String pageCounter = null;
            try
            {
                IPEndPoint endPoint = new IPEndPoint(printingDeviceIP, 161);
                OctetString octetString = new OctetString("public");
                List<Variable> inputVars = new List<Variable> { new Variable(new ObjectIdentifier(".1.3.6.1.2.1.43.10.2.1.4.1.1")) };

                List<Variable> outputVars = (List<Variable>)Messenger.Get(VersionCode.V1, endPoint, octetString, inputVars, 60000);
                if (outputVars.Count > 0)
                    pageCounter = outputVars[0].Data.ToString();
            }
            catch (Exception exc)
            {
                NotifyObject(exc.Message + exc.StackTrace);
            }

            return pageCounter;
        }

        private void FindDevices()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, 161);
            OctetString octetString = new OctetString("public");

            discoverer.AgentFound += new EventHandler<AgentFoundEventArgs>(DeviceFoundV1);
            discoverer.Discover(VersionCode.V1, endPoint, octetString, 9000);
            //discoverer.AgentFound += new EventHandler<AgentFoundEventArgs>(DeviceFoundV2);
            //discoverer.Discover(VersionCode.V2, endPoint, octetString, 9000);
            //discoverer.AgentFound += new EventHandler<AgentFoundEventArgs>(DeviceFoundV3);
            //discoverer.Discover(VersionCode.V3, endPoint, null, 9000);
        }

        private void DeviceFoundV1(Object sender, AgentFoundEventArgs e)
        {
            AddDevice("V1", e.Agent, e.Variable);
        }

        private void DeviceFoundV2(Object sender, AgentFoundEventArgs e)
        {
            AddDevice("V2", e.Agent, e.Variable);
        }

        private void DeviceFoundV3(Object sender, AgentFoundEventArgs e)
        {
            AddDevice("V3", e.Agent, e.Variable);
        }

        private void AddDevice(String snmpVersion, IPEndPoint agent, Lextm.SharpSnmpLib.Variable var)
        {
            int devicePageCounter = 0;
            String deviceInfo = "";
            if ((var != null) && (var.Data != null)) deviceInfo = var.Data.ToString();

            List<PrintingDevice> deviceMatches = deviceList.FindAll(delegate(PrintingDevice device) { return device.ipAddress.ToString() == agent.Address.ToString(); });
            if (deviceMatches.Count == 0)
            {
                String deviceDescription = GetDeviceDescription(agent.Address);
                String serialNumber = GetSerialNumber(agent.Address, deviceInfo);
                String pageCounter = GetPageCounter(agent.Address);

                Boolean deviceOK = true;
                if (String.IsNullOrEmpty(deviceDescription)) deviceOK = false;
                if (String.IsNullOrEmpty(serialNumber)) deviceOK = false;
                if (String.IsNullOrEmpty(pageCounter)) deviceOK = false;
                if (!int.TryParse(pageCounter, out devicePageCounter)) deviceOK = false;
                if (deviceOK) deviceList.Add(new PrintingDevice(this.tenantId, agent.Address.ToString(), deviceDescription, serialNumber, devicePageCounter));
            }
        }

        // Envia os dispositivos de impressão encontrados na rede
        public void Execute()
        {
            // Verifica os parâmetros recebidos
            if (taskParams == null) return;
            this.serviceUrl = taskParams["url"];
            this.tenantId = int.Parse(taskParams["tenantId"]);

            NotifyObject("Mapeando Dispositivos de Impressão. SNMP versão 1 porta 161");

            // Faz o mapeamento dos dispositivos físicos via SNMP
            FindDevices();

            // Envia os dados coletados ao servidor
            RequestHandler requestHandler = new RequestHandler(this.serviceUrl, 90000, this);
            Boolean requestSucceded = requestHandler.StartRequest("RegisterDevices", deviceList);
            if (!requestSucceded) NotifyObject("Não foi possível registrar dispositivos. Falha ao enviar requisição");
        }
    }

}
