# Modsフォルダに必要なファイル郡をコピーするスクリプト

# ユーザープロファイルのパスを指定
$userProfilePath = "C:\Users\wakad\AppData\Roaming\7DaysToDie\Mods\ascension-protocol"

# フォルダの存在確認と、あれば削除、なければ作成
if (Test-Path $userProfilePath) {
    Remove-Item -Recurse -Force $userProfilePath
}
New-Item -ItemType Directory -Path $userProfilePath

# フォルダ内のファイルをコピー(recurse)
Copy-Item -Path "Mods\ascension-protocol\*" -Destination $userProfilePath -Recurse

# ビルドしたdllをコピー
Copy-Item -Path "ascension-protocol\bin\Release\net481\ascension-protocol.dll" -Destination "$userProfilePath\ascension-protocol.dll"

# アセットバンドル
# フォルダの存在確認と、あれば削除、なければ作成
if (Test-Path "$userProfilePath\Bundles") {
    Remove-Item -Recurse -Force "$userProfilePath\Bundles"
}
New-Item -ItemType Directory -Path "$userProfilePath\Bundles"

Copy-Item -Path "Assets\AssetBundles\models.bundle" -Destination "$userProfilePath\Bundles\models.bundle"
Copy-Item -Path "Assets\AssetBundles\animations.bundle" -Destination "$userProfilePath\Bundles\animations.bundle"
