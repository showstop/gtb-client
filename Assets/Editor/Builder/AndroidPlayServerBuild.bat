@echo off
Echo [Process batchjob]===================================================== >> unitybuild.log
Echo %date% %time% [BUILD] Unity batch build - Android Player Server >> unitybuild.log
@"C:\\Program Files\Unity\Editor\Unity.exe" -quit -batchmode -projectPath D:\Work\gtb\trunk\client -nographics -executeMethod Builder.BuildPlayServerAndroid >> unitybuild.log
Echo %date% %time% [END] >> unitybuild.log