using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Adddiagnosis
/// </summary>
/// 
[Serializable]
public class AddDrug
{
    public AddDrug()
    {
        //
        // TODO: Add constructor logic here
        //

    }
    public string Medi_ID { get; set; }
    public string Medical { get; set; }
    public bool isChecked { get; set; }
    public string Medicine { get; set; }
    public string MedicationID { get; set; }
    public string Medicine_ID { get; set; }
    public bool IsChkd { get; set; }
}