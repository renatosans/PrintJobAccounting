using System;
using System.Web.UI;
using DocMageFramework.FileUtils;
using DocMageFramework.DataManipulation;


namespace WebAccounting
{
    public partial class IntegrationService : Page
    {
        private String action; // indica o método a ser chamado

        private DataAccess dataAccess;


        protected void Page_Load(object sender, EventArgs e)
        {
            // Verifica se o método foi informado
            if (String.IsNullOrEmpty(Request["action"]))
            {
                WriteResponse("Missing Argument.");
                return;
            }
            action = Request["action"];

            if (action == "GetBusinessPartner")
            {
                String cardCode = Request["cardCode"];
                WriteResponse(GetBusinessPartner(cardCode));
                return;
            }

            if (action == "GetAllBusinessPartners")
            {
                WriteResponse(GetAllBusinessPartners());
                return;
            }

            WriteResponse("Method not implemented.");
        }

        private String GetBusinessPartner(String cardCode)
        {
            StartDBAccess();
            String xml = "<cardName>Datacopy Trade</cardName><cardCode>" + cardCode + "</cardCode>";
            FinishDBAccess();

            return xml;
        }

        private String GetAllBusinessPartners()
        {
            StartDBAccess();
            String xml = "<BusinessPartners></BusinessPartners>";
            FinishDBAccess();

            return xml;
        }

        private void StartDBAccess()
        {
            // Abre a conexão com o banco
            dataAccess = DataAccess.Instance;
            dataAccess.MountConnection(FileResource.MapWebResource(Server, "DataAccess.xml"), DatabaseEnum.PrintAccounting);
            dataAccess.OpenConnection();
        }

        private void FinishDBAccess()
        {
            // Fecha a conexão com o banco
            dataAccess.CloseConnection();
            dataAccess = null;
        }

        private void WriteResponse(String response)
        {
            this.Page.Response.Clear();
            this.Page.Response.Write("<response>" + response + "</response>");
        }
    }

}
