![DL Count](https://img.shields.io/github/downloads/turtle-insect/OctopathTraveler/total.svg)

[![Build status](https://ci.appveyor.com/api/projects/status/p0qp4jhksi2j0ktq?svg=true)](https://ci.appveyor.com/project/turtle-insect/octopathtraveler)

# 寄付

<a href="https://www.buymeacoffee.com/06yi7RLlT" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/black_img.png" alt="Buy Me A Coffee" style="height: auto !important;width: auto !important;" ></a>

# Langage

[English](README_EN.md) [中文](README.md)

# 概要

Switch OCTOPATH TRAVELER(オクトパス トラベラー)のセーブデータ編集Tool

# ソフト

http://www.jp.square-enix.com/octopathtraveler/

# 実行に必要

* Windows マシン
* [.NET デスクトップ ランタイム 6.0](https://dotnet.microsoft.com/download), [Click Download](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* セーブデータの吸い出し
* セーブデータの書き戻し

# Build環境

* Windows 10(64bit)
* Visual Studio 2022

# 編集時の手順

* saveDataを吸い出す
  * 結果、以下が取得可能
    * KSSaveData1(KSSaveData2、KSSaveData3、、、)
    * KSSystemData
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を読み込む
* 任意の編集を行う
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を書き出す
* saveDataを書き戻す

# Special Thanks

* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
  * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
  * [Takumah](https://gbatemp.net/members/takumah.456165/)
  * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
  * [八方旅人全成就指南 - Steam社区](https://steamcommunity.com/sharedfiles/filedetails/?id=2795091350)
  * [Octopath Traveler Resource - Google Sheets](https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q)
  * [Octopath Traveler : TreasureStates - Google Sheets](https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg)
  * [八方旅人宝箱状态对照说明 - Baidu Tieba](https://tieba.baidu.com/p/7822253075)
  * [Octopath Traveler : MONSTERS ID LIST - Google Sheets](https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho)
