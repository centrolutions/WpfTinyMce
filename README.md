# WpfTinyMce
A control that allows you to embed an HTML editor in your application.

You will need to host a single page somewhere that displays a TinyMCE control.

# Your Web Page
index.html example:

```
<!DOCTYPE html>
<html>
<head>
  <script type="text/javascript" src="js/tinymce/tinymce.min.js"></script>
  <script type="text/javascript">
      tinymce.init({ selector: 'textarea', plugins: "autolink code image link lists table" });
  </script>
</head>
<body>
    <form action=".">
        <textarea></textarea>
    </form>
</body>
</html>
```

See the [TinyMCE Website](http://tinymce.com) for more details and information about styling the TinyMCE web control.

# Your WPF Window
Add a reference to WpfTinyMce to your project.
Add the following namespace to your WPF Window:
```
xmlns:mce="clr-namespace:WpfTinyMce;assembly=WpfTinyMce"
```

In your Window, use the control like:
```
<mce:TinyMce x:Name="tinyMce" HtmlValue="{Binding Html, Mode=TwoWay}" TinyMceUrl="http://yourwebsite.com/index.html" />
```