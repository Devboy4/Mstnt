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
using DevExpress.Web.ASPxSpellChecker;
using DevExpress.Web.ASPxEditors;

public partial class SpellCheckOptionsForm : SpellCheckerUserControl {
    protected void Page_Load(object sender, EventArgs e) {
        PrepareFormCheckBoxes();
        PrepareLanguagesCombo();
    }

    protected override ASPxEditBase[] GetChildDxEdits() {
        return new ASPxEditBase[] { chkbEmails, 
                                    chkbMixedCase, 
                                    chkbNumbers, 
                                    chkbUpperCase, 
                                    chkbUrls, 
                                    comboLanguage, 
                                    chkbTags 
        };
    }

    protected override ASPxButton[] GetChildDxButtons() {
        return new ASPxButton[] {
            btnCancel,
            btnOK
        };
    }

    protected void PrepareFormCheckBoxes() {
        ASPxSpellChecker spellChecker = ((ASPxSpellChecker)NamingContainer);
        chkbEmails.Checked = spellChecker.SettingsSpelling.IgnoreEmails;
        chkbMixedCase.Checked = spellChecker.SettingsSpelling.IgnoreMixedCaseWords;
        chkbNumbers.Checked = spellChecker.SettingsSpelling.IgnoreWordsWithNumber;
        chkbUpperCase.Checked = spellChecker.SettingsSpelling.IgnoreUpperCaseWords;
        chkbUrls.Checked = spellChecker.SettingsSpelling.IgnoreUrls;
        chkbTags.Checked = spellChecker.SettingsSpelling.IgnoreMarkupTags;
    }
    protected void PrepareLanguagesCombo() {
        comboLanguage.Items.Clear();
        for (int i = 0; i < SpellChecker.Dictionaries.Count; i++) {
            if (comboLanguage.Items.FindByText(SpellChecker.Dictionaries[i].GetCulture().DisplayName) == null)
                comboLanguage.Items.Add(SpellChecker.Dictionaries[i].GetCulture().DisplayName);
        }
        comboLanguage.SelectedItem = comboLanguage.Items.FindByText(SpellChecker.GetCulture().DisplayName);
    }
}
