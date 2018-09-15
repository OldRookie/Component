%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe ..\CTF.Job.WindowService.exe
Net Start CTFScheduleService
sc config CTFScheduleService start= auto
pause