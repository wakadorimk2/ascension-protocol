# 変数設定
$projectRoot = "C:\Users\wakad\Projects\ascension-protocol"  # プロジェクトのルートパス
$dllSourcePath = "$projectRoot\ascension-protocol\bin\Release\net481"  # DLLのビルドパス
$fbxSourcePath = "$projectRoot\Assets\AssetBundles"  # アセットバンドルのあるパス
$modsOutputPath = "C:\Users\wakad\AppData\Roaming\7DaysToDie\Mods"  # Modsフォルダのパス
$modFolderName = "AscensionProtocol"  # Modsフォルダ内のフォルダ名

# 古いModsフォルダを削除
Write-Host "古いModsフォルダを削除中..."
Remove-Item -Recurse -Force "$modsOutputPath\$modFolderName"

# Modsフォルダが存在しない場合は作成する
if (-Not (Test-Path "$modsOutputPath\$modFolderName")) {
    Write-Host "Modsフォルダが存在しません。作成します: $modsOutputPath\$modFolderName"
    New-Item -Path "$modsOutputPath\$modFolderName" -ItemType Directory
}

# DLLファイルをコピー
Write-Host "DLLファイルをコピー中..."
Copy-Item "$dllSourcePath\ascension-protocol.dll" -Destination "$modsOutputPath\$modFolderName" -Force

# アセットバンドルをコピー
Write-Host "アセットバンドルをコピー中..."
Copy-Item "$fbxSourcePath\*.bundle" -Destination "$modsOutputPath\$modFolderName" -Force

# ModInfo.xmlとその他のXMLファイルをコピー
Write-Host "XMLファイルをコピー中..."
Copy-Item "$projectRoot\ModInfo.xml" -Destination "$modsOutputPath\$modFolderName" -Force

# Configフォルダ内のファイルをコピー
Write-Host "Configフォルダ内のファイルをコピー中..."
if (-Not (Test-Path "$modsOutputPath\$modFolderName\Config")) {
    Write-Host "Configフォルダが存在しません。作成します: $modsOutputPath\$modFolderName\Config"
    New-Item -Path "$modsOutputPath\$modFolderName\Config" -ItemType Directory
}
Copy-Item "$projectRoot\Config\*.xml" -Destination "$modsOutputPath\$modFolderName\Config" -Force

# 完了メッセージ
Write-Host "コピーが完了しました。Modsフォルダに必要なファイルが揃いました。"
