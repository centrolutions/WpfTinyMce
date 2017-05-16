using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using mshtml;
using System.Windows.Threading;

namespace WpfTinyMce
{
    public partial class TinyMce : UserControl
    {
        private SHDocVw.IWebBrowser2 BrowserControl2;

        public string TinyMceUrl
        {
            get { return (string)GetValue(TinyMceUrlProperty); }
            set { SetValue(TinyMceUrlProperty, value); }
        }
        public static readonly DependencyProperty TinyMceUrlProperty =
            DependencyProperty.Register("TinyMceUrl", typeof(string), typeof(TinyMce), new PropertyMetadata(string.Empty, TinyMceUrlPropertyChanged));

        private static void TinyMceUrlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = (TinyMce)d;
            ctrl.Browser.Navigate(e.NewValue as string);
        }

        public string HtmlValue
        {
            get { return (string)GetValue(HtmlValueProperty);  }
            set { SetValue(HtmlValueProperty, value); }
        }
        public static readonly DependencyProperty HtmlValueProperty =
            DependencyProperty.Register("HtmlValue", typeof(string), typeof(TinyMce), new PropertyMetadata(string.Empty, HtmlValueProperty_Changed));

        private static void HtmlValueProperty_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var tinyMce = d as TinyMce;
            var currentBrowserValue = tinyMce.GetTinyMceValue();

            //try to figure out if the new value came from the owner of the control (or a binding)
            if (e.NewValue as string != currentBrowserValue)
                tinyMce.SetTinyMceValue(e.NewValue as string);
        }

        //used to update the HtmlValue dependency property -- tried using javascript events, but they were unreliable
        private DispatcherTimer _UpdateTimer;

        public TinyMce()
        {
            InitializeComponent();

            //get a reference to the underlying activex control of the webbrowser
            BrowserControl2 = typeof(WebBrowser).GetProperty("AxIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(Browser, null) as SHDocVw.IWebBrowser2;

            //supress javascript errors
            BrowserControl2.Silent = true;

            _UpdateTimer = new DispatcherTimer() { IsEnabled = false };
            _UpdateTimer.Interval = TimeSpan.FromMilliseconds(500);
            _UpdateTimer.Tick += UpdateTimer_Tick;
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            HtmlValue = GetTinyMceValue();
        }

        private string GetTinyMceValue()
        {
            try
            {
                var htmlObj = Browser.InvokeScript("getHtml");
                return htmlObj as string;
            }
            catch { return string.Empty; }
        }

        private void SetTinyMceValue(string html)
        {
            try
            {
                Browser.InvokeScript("setHtml", html);
            }
            catch { }
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            var browser = (WebBrowser)sender;
            mshtml.HTMLDocument doc = (mshtml.HTMLDocument)browser.Document;
            InjectScript(doc);

            _UpdateTimer.Start();
        }

        private void InjectScript(mshtml.HTMLDocument doc)
        {
            //create a new script element
            var element = doc.createElement("script") as mshtml.IHTMLElement;
            var script = (mshtml.IHTMLScriptElement)element;
            script.text = Properties.Resources.GetSetFunctions;
            script.type = "text/javascript";

            //add the new script after the first child in the body of the document
            var body = doc.body as mshtml.IHTMLElement;
            var firstChild = (mshtml.IHTMLElement2)body.children[0];
            firstChild.insertAdjacentElement("afterend", (mshtml.IHTMLElement)script);
        }
    }
}
