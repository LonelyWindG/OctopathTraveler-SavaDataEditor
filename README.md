# 八方旅人存档编辑器

[![GitHub release](https://img.shields.io/github/v/release/LonelyWindG/OctopathTraveler-SavaDataEditor?style=for-the-badge)](https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/turtle-insect/OctopathTraveler/total?style=for-the-badge&color=00B000)](https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LonelyWindG/OctopathTraveler-SavaDataEditor?style=for-the-badge&color=blue)](https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor/blob/master/LICENSE)

## 概要

适用于Steam, Xbox PC, Nintendo Switch等平台的八方旅人游戏存档

## 语言

[日本語](README_JA.md) [English](README_EN.md)

## 游戏官网

https://octopathtraveler.nintendo.com/

http://www.jp.square-enix.com/octopathtraveler/

## 运行要求

* Windows操作系统
* [.NET 桌面运行时 6.0](https://dotnet.microsoft.com/download), [点此下载](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* 八方旅人存档文件
  * Steam存档路径%USERPROFILE%/Documents/My Games/Octopath_Traveler/(数字编号)
    /SaveGames, 在SaveGames文件夹里SaveData0对应着游戏的自动存档. SaveData1/2/3对应相应顺序的存档
  * Xbox PC存档路径%LOCALAPPDATA%/Packages/39EA002F.FrigateMS_n746a19ndrrjg/SystemAppData/wgs/(一串字母数字)/(一串字母数字), 在该文件夹内大小为2099KB的几个文件为每个存档位的存档文件;要快速找到要修改的存档, 可以通过进入游戏内保存一个存档, 此目录内修改日期最近的那个就是
  * Nintendo Switch, 从机器中导入导出存档

## Build环境

* Windows 10 (64位)
* Visual Studio 2022

## 特别感谢

* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
  * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
  * [Takumah](https://gbatemp.net/members/takumah.456165/)
  * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
  * [SUDALV92](https://github.com/SUDALV92)
  * [八方旅人全成就指南 - Steam社区](https://steamcommunity.com/sharedfiles/filedetails/?id=2795091350)
  * [八方旅人全资源清单 - Google表格](https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q)
  * [八方旅人宝箱状态对照表 - Google表格](https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg)
  * [八方旅人宝箱状态对照说明 - 百度贴吧](https://tieba.baidu.com/p/7822253075)
  * [八方旅人怪物ID对照表 - Google表格](https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho)
  * [GVAS游戏存档转换工具](https://github.com/januwA/gvas-converter)
