/*
{************************************************************************************}
{                                                                                    }
{   DO NOT MODIFY THIS FILE!                                                         }
{                                                                                    }
{   It will be overwritten without prompting when a new version becomes              }
{   available. All your changes will be lost.                                        }
{                                                                                    }
{   This file contains the default template and is required for the form             }
{   rendering. Improper modifications may result in incorrect behavior of            }
{   dialog forms.                                                                    }
{                                                                                    }
{************************************************************************************}
*/
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxHtmlEditor;
using DevExpress.Web.ASPxClasses;
using DevExpress.Web.ASPxEditors;

public partial class ImagePropertiesForm : HtmlEditorUserControl {
    protected void Page_Load(object sender, EventArgs e) {
        ckbCreateThumbnail.ClientVisible = !string.IsNullOrEmpty(HtmlEditor.SettingsImageUpload.UploadImageFolder);
        CreateConstrainProportionsImages();
    }
    protected override ASPxEditBase[] GetChildDxEdits() {
        return new ASPxEditBase[] {lblSize, cmbSize,
                lblWidth,  spnWidth, 
                lblPixelWidth, lblHeight, 
                spnHeight, lblPixelHeight, lblImagePosition,
                cmbImagePosition, lblImageDescription,
                txbDescription, ckbCreateThumbnail, lblThumbnailFileName, txbThumbnailFileName };
    }
    protected void CreateConstrainProportionsImages() {
        Image imgTop = new Image();
        HtmlEditor.GetInsertImageDialogConstrainProportionsTop().AssignToControl(imgTop, false);
        cellContarainTop.Controls.Add(imgTop);

        Image imgBottom = new Image();
        HtmlEditor.GetInsertImageDialogConstrainProportionsBottom().AssignToControl(imgBottom, false);
        cellContarainBottom.Controls.Add(imgBottom);

        Image imgSwitcherOn = new Image();
        imgSwitcherOn.ID = "imgOn";
        HtmlEditor.GetInsertImageDialogConstrainProportionsMiddleOn().AssignToControl(imgSwitcherOn, false);
        cellContarainSwitcher.Controls.Add(imgSwitcherOn);

        Image imgSwitcherOff = new Image();
        imgSwitcherOff.ID = "imgOff";
        HtmlEditor.GetInsertImageDialogConstrainProportionsMiddleOff().AssignToControl(imgSwitcherOff, false);
        cellContarainSwitcher.Controls.Add(imgSwitcherOff);

        imgSwitcherOn.Attributes.Add("onclick", "aspxConstrainProportionsSwicthClick(event, '" + imgSwitcherOff.ClientID + "')");
        imgSwitcherOff.Attributes.Add("onclick", "aspxConstrainProportionsSwicthClick(event,'" + imgSwitcherOn.ClientID + "')");
        imgSwitcherOff.Style.Add("display", "none");
    }
}
