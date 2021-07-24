@echo off
set /p port="Enter localhost port: "
start cmd.exe /c myNgrok YOUR_API_KEY & ngrok authtoken YOUR_NGROK_TOKEN & ngrok http %port% -host-header="localhost:%port%" 
