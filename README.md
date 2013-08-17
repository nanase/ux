# ux - Micro Xylph

**バージョン: v0.1.5-dev**

ux は軽量でシンプルな動作を目標としたソフトウェアシンセサイザです。C# で作られており、Mono 上でも動作します。


## 概要

ux は Xylph (シルフ) の後継として開発されています。Xylph の開発で得られた最低限必要な機能を絞り、なおかつ Xylph よりも軽快に動作するよう設計されています。

ux は モノフォニック、複数パート、ポルタメント、ビブラートなどの機能を持ち、音源として矩形波、16 段三角波、ユーザ波形、線形帰還シフトレジスタによる擬似ノイズ、4 オペレータ FM 音源を搭載しています。


## 動作確認
* Mono 2.10.8.1 (Linux Mint 14 64 bit)
* .NET Framework 4.5 (Windows 7 64 bit)
* (内部プロジェクトは互換性を理由に .NET Framework 4.0 をターゲットにしています)


## v0.1.5-devからの主な変更点

* 修正 - Sequencer クラスで別スレッドへのイベントハンドルを修正
* 修正 - MidiConnector クラスがドラムチャネルを擬似ポリフォニックで演奏するよう修正
* 修正 - メソッド FM.Generate の virtual キーワードを修正
* 修正 - 音源切替時の処理を高速化
* 修正 - FM音源が初期化時またはリセット時に正しく最適化されていなかった問題を修正
* 修正 - MidiConnector クラスが正しく解放されていなかった問題を修正
* 修正 - メソッド MidiConnector.Reset を public に変更
* 修正 - Sequencer クラスにおいてイベントを呼び出したスレッドで停止できないよう修正
* 追加 - SmfConnector クラスに Sequence, Sequencer プロパティを追加
* 追加 - メソッド IWaveform.Reset を追加
* 追加 - Master クラスに PartCount, ToneCount プロパティを追加
* 追加 - uxPlayer プロジェクトの追加


## 使用素材
uxPlayer にて [p.yusukekamiyamane.com](http://p.yusukekamiyamane.com/) の Fugue Icons を使用しています。

(C) 2013 Yusuke Kamiyamane. All rights reserved.

These icons are licensed under a Creative Commons
Attribution 3.0 License.
<http://creativecommons.org/licenses/by/3.0/>

## ライセンス
Copyright &copy; 2013 Tomona Nanase.

MIT ライセンス
