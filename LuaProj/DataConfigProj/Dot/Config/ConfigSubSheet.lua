require ("Dot/Config/ConfigConst")
require ("Dot/Config/ConfigSheetLine")

local rawget = rawget
local rawset = rawset
local getmetatable = getmetatable
local setmetatable = setmetatable

ConfigSubSheet = {}

function ConfigSubSheet:SetDepend(depends)
  if depends then
    rawset(self,ConfigConst.DEPENDS_NAME,depends)
  end
end

function ConfigSubSheet:GetDataById(dataId)
  if not self or type(self) ~= "table" or not dataId or type(dataId) ~= "number" then
    return nil
  end
  
  local data = rawget(self,dataId)
  if not data then
    return nil
  end
  
  if not getmetatable(data) then
    setmetatable(data,ConfigSheetLine)
    local depends = rawget(self,ConfigConst.DEPENDS_NAME)
    data:SetDepend(depends)
  end
  
  return data
end

ConfigSubSheet.__index = ConfigSubSheet
ConfigSubSheet.__newindex = function(t,k,v)
  error("Read only,so you can't add new key for it")
end
ConfigSubSheet.__metatable = "locked"