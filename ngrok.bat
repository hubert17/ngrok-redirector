@echo off
set /p port="Enter localhost port: "
set /p user="Enter user (leave blank if with valid apikey): "
start cmd.exe /c myNgrok YOUR_API_KEY %user% & ngrok authtoken YOUR_NGROK_TOKEN & ngrok http %port% -host-header="localhost:%port%" 
