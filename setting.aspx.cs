using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class setting : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            bindValues();
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            string fileName = Server.MapPath("~/Template/Default_Admin.xml");
            xmlDoc.Load(fileName);
            XmlNode node = xmlDoc.DocumentElement.SelectSingleNode("/Defaults/Settings");

            node.SelectSingleNode("forwardCC").InnerText = chkCC.Checked.ToString();
            node.SelectSingleNode("forwardPE").InnerText = chkPE.Checked.ToString();
            node.SelectSingleNode("forwardROM").InnerText = chkROM.Checked.ToString();
            node.SelectSingleNode("is_no_diagnostudy_show").InnerText = chk_is_show.Checked.ToString();
            node.SelectSingleNode("is_no_diagnostudy_msg").InnerText = txt_no_daigno_msg.Text;

            XmlNode nodeSetting = xmlDoc.DocumentElement.SelectSingleNode("/Defaults/Settings/POCReports");

            nodeSetting.SelectSingleNode("Sex").InnerText = chkSex.Checked.ToString();
            nodeSetting.SelectSingleNode("FName").InnerText = chkFName.Checked.ToString();
            nodeSetting.SelectSingleNode("LName").InnerText = chkLName.Checked.ToString();

            nodeSetting.SelectSingleNode("Case").InnerText = chkCase.Checked.ToString();
            nodeSetting.SelectSingleNode("DOB").InnerText = chkDOB.Checked.ToString();
            nodeSetting.SelectSingleNode("DOA").InnerText = chkDOA.Checked.ToString();
            nodeSetting.SelectSingleNode("MCode").InnerText = chkMCode.Checked.ToString();

            nodeSetting.SelectSingleNode("Phone").InnerText = chkPhone.Checked.ToString();
            nodeSetting.SelectSingleNode("Location").InnerText = chkLocation.Checked.ToString();
            nodeSetting.SelectSingleNode("Policy_No").InnerText = chkPolicy_No.Checked.ToString();
            nodeSetting.SelectSingleNode("ClaimNumber").InnerText = chkClaim_No.Checked.ToString();
            nodeSetting.SelectSingleNode("Insurance").InnerText = chkInsurance.Checked.ToString();

            nodeSetting.SelectSingleNode("Consider").InnerText = chkConsider.Checked.ToString();
            nodeSetting.SelectSingleNode("Vaccinated").InnerText = chkVaccinated.Checked.ToString();
            nodeSetting.SelectSingleNode("Requested").InnerText = chkRequested.Checked.ToString();
            nodeSetting.SelectSingleNode("Scheduled").InnerText = chkScheduled.Checked.ToString();
            nodeSetting.SelectSingleNode("Executed").InnerText = chkExecuted.Checked.ToString();
            nodeSetting.SelectSingleNode("MC").InnerText = chkMC.Checked.ToString();
            nodeSetting.SelectSingleNode("Attorny").InnerText = chkAttory.Checked.ToString();

            xmlDoc.Save(fileName);
            divSuccess.Attributes.Add("style", "display:block");
            divfail.Attributes.Add("style", "display:none");
        }
        catch (Exception ex)
        {
            divfail.Attributes.Add("style", "display:block");
            divSuccess.Attributes.Add("style", "display:none");
        }
    }

    private void bindValues()
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Server.MapPath("~/Template/Default_Admin.xml"));
            XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings");
            foreach (XmlNode node in nodeList)
            {
                chkCC.Checked = node.SelectSingleNode("forwardCC") == null ? chkCC.Checked : Convert.ToBoolean(node.SelectSingleNode("forwardCC").InnerText);
                chkPE.Checked = node.SelectSingleNode("forwardPE") == null ? chkPE.Checked : Convert.ToBoolean(node.SelectSingleNode("forwardPE").InnerText);
                chkROM.Checked = node.SelectSingleNode("forwardROM") == null ? chkPE.Checked : Convert.ToBoolean(node.SelectSingleNode("forwardROM").InnerText);
                chk_is_show.Checked = node.SelectSingleNode("is_no_diagnostudy_show") == null ? chk_is_show.Checked : Convert.ToBoolean(node.SelectSingleNode("is_no_diagnostudy_show").InnerText);
                txt_no_daigno_msg.Text = node.SelectSingleNode("is_no_diagnostudy_msg").InnerText;
            }

            nodeList = xmlDoc.DocumentElement.SelectNodes("/Defaults/Settings/POCReports");

            foreach (XmlNode node in nodeList)
            {
                chkSex.Checked = node.SelectSingleNode("Sex") == null ? chkSex.Checked : Convert.ToBoolean(node.SelectSingleNode("Sex").InnerText);
                chkFName.Checked = node.SelectSingleNode("FName") == null ? chkFName.Checked : Convert.ToBoolean(node.SelectSingleNode("FName").InnerText);
                chkLName.Checked = node.SelectSingleNode("LName") == null ? chkFName.Checked : Convert.ToBoolean(node.SelectSingleNode("LName").InnerText);
                chkAttory.Checked = node.SelectSingleNode("Attorny") == null ? chkAttory.Checked : Convert.ToBoolean(node.SelectSingleNode("Attorny").InnerText);
                chkMC.Checked = node.SelectSingleNode("MC") == null ? chkMC.Checked : Convert.ToBoolean(node.SelectSingleNode("MC").InnerText);
                chkCase.Checked = node.SelectSingleNode("Case") == null ? chkCase.Checked : Convert.ToBoolean(node.SelectSingleNode("Case").InnerText);
                chkDOB.Checked = node.SelectSingleNode("DOB") == null ? chkDOB.Checked : Convert.ToBoolean(node.SelectSingleNode("DOB").InnerText);
                chkDOA.Checked = node.SelectSingleNode("DOA") == null ? chkDOA.Checked : Convert.ToBoolean(node.SelectSingleNode("DOA").InnerText);
                chkMCode.Checked = node.SelectSingleNode("MCode") == null ? chkMCode.Checked : Convert.ToBoolean(node.SelectSingleNode("MCode").InnerText);
                chkPhone.Checked = node.SelectSingleNode("Phone") == null ? chkPhone.Checked : Convert.ToBoolean(node.SelectSingleNode("Phone").InnerText);
                chkLocation.Checked = node.SelectSingleNode("Location") == null ? chkLocation.Checked : Convert.ToBoolean(node.SelectSingleNode("Location").InnerText);
                chkPolicy_No.Checked = node.SelectSingleNode("Policy_No") == null ? chkPolicy_No.Checked : Convert.ToBoolean(node.SelectSingleNode("Policy_No").InnerText);
                chkClaim_No.Checked = node.SelectSingleNode("ClaimNumber") == null ? chkClaim_No.Checked : Convert.ToBoolean(node.SelectSingleNode("ClaimNumber").InnerText);
                chkInsurance.Checked = node.SelectSingleNode("Insurance") == null ? chkInsurance.Checked : Convert.ToBoolean(node.SelectSingleNode("Insurance").InnerText);
                chkConsider.Checked = node.SelectSingleNode("Consider") == null ? chkConsider.Checked : Convert.ToBoolean(node.SelectSingleNode("Consider").InnerText);
                chkVaccinated.Checked = node.SelectSingleNode("Vaccinated") == null ? chkVaccinated.Checked : Convert.ToBoolean(node.SelectSingleNode("Vaccinated").InnerText);
                chkRequested.Checked = node.SelectSingleNode("Requested") == null ? chkRequested.Checked : Convert.ToBoolean(node.SelectSingleNode("Requested").InnerText);
                chkScheduled.Checked = node.SelectSingleNode("Scheduled") == null ? chkScheduled.Checked : Convert.ToBoolean(node.SelectSingleNode("Scheduled").InnerText);
                chkExecuted.Checked = node.SelectSingleNode("Executed") == null ? chkExecuted.Checked : Convert.ToBoolean(node.SelectSingleNode("Executed").InnerText);

            }

        }
        catch (Exception ex)
        {
        }

    }
}