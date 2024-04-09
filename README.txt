To run the program, go to this link:

http://sts7534.cloud.sci.uwo.ca/ssbuild/index.html

Note: The website is only accessible from within the UWO network (e.g. on campus WiFi, no VPN).


Alternatively, you can host the website yourself using Python.

In the "ssbuild" directory (included in OWL submission), run this command:
    python3 -m http.server -b localhost 5001

Use this command on Windows Powershell:
    python -m http.server -b localhost 5001

Then go to this link:
    localhost:5001

