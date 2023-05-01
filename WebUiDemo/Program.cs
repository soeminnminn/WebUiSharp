// See https://aka.ms/new-console-template for more information
using WebUiSharp;

#if DEBUG
Console.WriteLine("Start WebUI window!");
#endif

string HTML = @"
<!DOCTYPE html>
<html>
    <head>
    <title>Dashboard</title>
    <style>
        body {
        color: white;
        background: #0F2027;
        background: -webkit-linear-gradient(to right, #4e99bb, #2c91b5, #07587a);
        background: linear-gradient(to right, #4e99bb, #2c91b5, #07587a);
        text-align: center;
        font-size: 18px;
        font-family: sans-serif;
        }
    </style>
    </head>
    <body>
    <h1>Welcome !</h1>
    <br>
    <br>
    <button id='Exit'>Exit</button>
    </body>
</html>";

using (var app = new WebUiApplication())
{
    var win = app.NewWindow();
    win.Show(HTML);

    win.Bind("Exit", (WebUiEvent e) => 
    {
        app.Exit();
    });

    app.Wait();
}
