using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Adddiagnosis
/// </summary>
/// 
[Serializable]
public class Adddiagnosis
{
	public Adddiagnosis()
	{
		//
		// TODO: Add constructor logic here
		//
      
	}

    public string DiagCode_ID { get; set; }
    public string BodyPart { get; set; }
    public string Description { get; set; }
    public string DiagCode { get; set; }
    public bool isChecked { get; set; }
    public bool PN { get; set; }
    public string Diag_Master_ID { get; set; }
}