#Login-AzAccount -Subscription "f124b668-7e3d-4b53-ba80-09c364def1f3"
Function Invoke-API {
    Param(
        [parameter(Mandatory = $true)][string]$Url,
        [parameter(Mandatory = $true)][string]$Method,
        [parameter(Mandatory = $false)][string]$Body,
        [parameter(Mandatory = $false)][string]$ContentType
    )

    $token = Get-AzAccessToken -ResourceUrl "https://azappconfig-sponsor.azconfig.io"
    $apiHeaders = @{
        Authorization="Bearer $($token.Token)"
    }
    
    Write-Verbose "Trying to invoke api: $Url"

    try {
        if ($Body) {
            $result = Invoke-RestMethod -Uri $Url -Headers $apiHeaders -Method $Method -ContentType $ContentType -Body $Body
        }
        else {
            $result = Invoke-RestMethod -Uri $Url -Headers $apiHeaders -Method $Method
        }
    }
    catch [System.Net.WebException] {
        $ex = $_.Exception
        try {
            if ($null -ne $ex.Response) {
                $streamReader = [System.IO.StreamReader]::new($_.Exception.Response.GetResponseStream())
                $errorContent = $streamReader.ReadToEnd() | ConvertFrom-Json

                if ($errorContent.error.code -eq "AddingAlreadyExistsGroupUserNotSupportedError") {
                    $existUser = $true
                }
            }

            if ($existUser) {
                Write-Warning "User already exists. Updating an existing user is not supported"
            }
            else {
                $message = $errorContent.error.code
                if ($message) {
                    Write-Error $message
                    exit
                }
                else {
                    Write-Error -Exception $ex
                    exit
                }
            }
        }
        catch {
            throw;
        }
        finally {
            if ($reader) { $reader.Dispose() }
            if ($stream) { $stream.Dispose() }
        }
    }
    return $result
}

$endpoint = "https://azappconfig-sponsor.azconfig.io"
$url = "$($endpoint)/kv/DemoPS:Message?api-version=1.0"

Invoke-API -Url $url -Method "GET"

