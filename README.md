# Ascension Protocol MOD - 基本設計と仕様決定ドキュメント

## プロジェクト概要
Ascension Protocolは、7 Days to Dieのゲームプレイを拡張するサイバーパンク風のMODプロジェクトです。このMODは、プレイヤーが荒廃した世界で生き残りながら、新たな文明の柱を構築することを目的としています。MODの主な特徴は、サバイバル要素と緻密に設計された建築システムの融合であり、プレイヤーは多彩な挑戦を乗り越えながら、タワーやその他のエンドコンテンツを構築し、進化させていくことが求められます。

## 主なコンセプトとテーマ
- サイバーパンクの美学: 世界は荒廃し、プレイヤーはその中でテクノロジーを駆使して新たな文明を築くことがテーマ。デジタル要素やネオンライト、サイバー空間を連想させるデザインが中心。
- タワー建設: プレイヤーはサイバーパンク風のタワーを建設し、その成長に応じて新たな機能や挑戦がアンロックされる。タワーは防衛拠点としても活用され、襲撃から守る役割を果たす。
- 柔軟なエンドコンテンツ: タワーに限定せず、エンドコンテンツはプレイヤーの選択や進行に応じて変化する可能性を持たせる。これにより、プレイヤーは自由度の高いプレイを楽しめる。

## 主要機能
1. タワーの建設と成長
   - 段階的な建設: タワーは複数の階層で構成され、各階層ごとに異なる機能を持たせる。進行に応じて、エネルギー供給、防衛システム、サイバーネットワークの中枢が追加されていく。
   - 防衛機能: 7日ごとに襲撃が発生し、タワーの防衛が必要。自動防衛システムやカスタムトラップなどが用意される。

2. MODの柔軟性
   - エンドコンテンツ: タワーの完成以外にも、デジタルワールドの修復や、サイバーウェーブの封印など、複数のエンドコンテンツを設けることで、プレイヤーが選択肢を持てるようにする。

3. ビジュアルとサウンドデザイン
   - サイバーパンク風のデザイン: タワーやUI、アイテムなどにサイバーパンクの要素を取り入れ、全体の美学を統一。ネオンライトやデジタルパターンが特徴的。
   - オリジナルサウンドトラック: サイバーパンクの雰囲気に合ったBGMや効果音を導入し、没入感を高める。

## 開発環境とツール
- GitHub: バージョン管理にはGitHubを使用。レポジトリ名は`ascension-protocol`。
- Linear: タスク管理ツールとしてLinearを採用。MODのタスクを細分化し、優先順位をつけて管理。
- Image Generator: アイコンやビジュアル素材は、AIを活用して生成。プロンプトを工夫してサイバーパンクの雰囲気を引き出す。

## 初期タスク
1. MODの基本設計と仕様決定
   - コンセプト、主な機能、エンドコンテンツの概要を決定。
   - MODのデザインやサウンドの方向性を確定。
   
2. ModInfo.xmlの作成
   - MODが正しく認識されるように、基本的な情報を記載した`ModInfo.xml`ファイルを作成。

3. アイテムとブロックの定義
   - ゲームに追加するカスタムアイテムやブロックの定義を`items.xml`や`blocks.xml`に記述。

4. 初期クエストと進行システムの作成
   - プレイヤーがMODの導入部分で体験する初期クエストやゲーム進行システムを設計・実装。

5. テスト環境の構築
   - MODの動作確認のためにテスト環境をセットアップし、初期テストを実施。
