using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Adddiagnosis
/// </summary>
public class AddStandards_Pro
{
    public AddStandards_Pro()
    {
        //
        // TODO: Add constructor logic here
        //

    }
    public string Procedure_ID { get; set; }
    public string MCODE { get; set; }
    public string BodyPart { get; set; }
    public string Heading { get; set; }
    public string CCDesc { get; set; }
    public string PEDesc { get; set; }
    public string ADesc { get; set; }
    public string PDesc { get; set; }
    public bool CF { get; set; }
    public bool isChecked { get; set; }
    public bool PN { get; set; }
}