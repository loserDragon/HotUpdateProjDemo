
local string loc_sceneName
local string loc_packageName
local string loc_assetName

local abMgrObj = CS.SUIFW.ABMgr.GetInstance()
local util = require(modname"util.lua")

local function LoadAssetBundlePack(sceneName,packageName,assetName)
    loc_sceneName = sceneName
    loc_packageName = packageName
    loc_assetName = assetName
    abMgrObj:LoadAssetBundlePack(loc_sceneName,loc_packageName,LoadComplete)
end

local function LoadComplete()
    local assetobj =  abMgrObj:LoadAsset(loc_sceneName,loc_packageName,loc_assetName,false)
    if assetobj then  
        CS.UnityEngine.Object.Instantiate(assetobj)
        print("CouLaunchABFW.lua/LoadComplete()/加载成功！！！")  
    else 
        print("### CouLaunchABFW.lua/LoadComplete()/加载资源出错！！！")  
    end
end

return {
   yield_return =  util.async_to_sync(LoadAssetBundlePack)

}