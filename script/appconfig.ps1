Function Get-AzAppConfigurationKey {
    Param(
        [parameter(Mandatory = $true)][string]$AppConfiguration,
        [parameter(Mandatory = $true)][string]$Key
    )

    $url = "$($AppConfiguration)/kv/$($Key)?api-version=1.0"

    $token = Get-AzAccessToken -ResourceUrl $AppConfiguration
    $apiHeaders = @{
        Authorization="Bearer $($token.Token)"
    }
    
    Write-Verbose "Trying to invoke app configuration rest api: $url"

    $result = Invoke-RestMethod -Uri $url -Headers $apiHeaders -Method "Get"
    
    return $result
}

$endpoint = "https://azappconfig-sponsor.azconfig.io"
Get-AzAppConfigurationKey -AppConfiguration $endpoint -Key "DemoPS:Message"

