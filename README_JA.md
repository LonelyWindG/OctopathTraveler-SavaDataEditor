# OCTOPATH TRAVELER(オクトパス トラベラー)のセーブデータ編集Tool

[![GitHub release](https://img.shields.io/github/v/release/LonelyWindG/OctopathTraveler-SavaDataEditor?style=for-the-badge)](https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/turtle-insect/OctopathTraveler/total?style=for-the-badge&color=00B000)](https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LonelyWindG/OctopathTraveler-SavaDataEditor?style=for-the-badge&color=blue)](https://github.com/LonelyWindG/OctopathTraveler-SavaDataEditor/blob/main/LICENSE)
[![Donate](https://img.shields.io/badge/Buy%20Me%20A%20Coffee-Donate-orange?style=for-the-badge&logo=buymeacoffee)](https://www.buymeacoffee.com/06yi7RLlT)

## Language

[English](README_EN.md) [中文](README.md)

## 概要

Steam, Xbox PC, Nintendo Switch OCTOPATH TRAVELER(オクトパス トラベラー)のセーブデータ編集Tool

## ソフト

http://www.jp.square-enix.com/octopathtraveler

## 実行に必要

* Windows マシン
* [.NET デスクトップ ランタイム 6.0](https://dotnet.microsoft.com/download), [クリックダウンロード](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* セーブデータの吸い出し
* セーブデータの書き戻し

## Build環境

* Windows 10(64bit)
* Visual Studio 2022

## Nintendo Switch用編集の手順

* SaveDataを吸い出す
  * 結果、以下が取得可能
    * KSSaveData1(KSSaveData2、KSSaveData3、、、)
    * KSSystemData
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を読み込む
* 任意の編集を行う
* KSSaveData1(KSSaveData2、KSSaveData3、、、)を書き出す
* SaveDataを書き戻す

## Special Thanks

* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
  * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
  * [Takumah](https://gbatemp.net/members/takumah.456165/)
  * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
  * [八方旅人全成就指南 - Steam Community](https://steamcommunity.com/sharedfiles/filedetails/?id=2795091350)
  * [Octopath Traveler Resource - Google Sheets](https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q)
  * [Octopath Traveler : TreasureStates - Google Sheets](https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg)
  * [八方旅人宝箱状态对照说明 - Baidu Tieba](https://tieba.baidu.com/p/7822253075)
  * [Octopath Traveler : MONSTERS ID LIST - Google Sheets](https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho)
