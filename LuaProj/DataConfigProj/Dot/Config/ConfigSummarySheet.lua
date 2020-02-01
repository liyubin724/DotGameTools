require ("Dot/Config/ConfigConst")
require("Dot/Config/ConfigSubSheet")

local rawget = rawget
local rawset = rawset
local setmetatable = setmetatable

ConfigSummarySheet = {}

----private----
--
--@param subSheet
--@return 
function ConfigSummarySheet:GetOrLoadSubSheet(subSheet)
  if not self or type(self) ~= "table" or not subSheet or type(subSheet) ~= "table" then
    return nil
  end
  
  local subSheetData = subSheet[ConfigConst.SUBSHEET_DATA_NAME]
  if not subSheetData then 
    subSheetData = require(subSheet[ConfigConst.SUBSHEET_PATH_NAME])
    rawset(subSheet,ConfigConst.SUBSHEET_DATA_NAME,subSheetData)
    
    setmetatable(subSheetData,ConfigSubSheet)
    
    local depends = rawget(self,ConfigConst.DEPENDS_NAME)
    subSheetData:SetDepend(depends)
    
  end
  
  return subSheetData
end


----public----
--
--@param dataId
--@return t
function ConfigSummarySheet:GetData(dataId)
  if not self or type(self) ~= "table" or not dataId or type(dataId) ~= "number" then
    return nil
  end

  local subSheets = rawget(self,ConfigConst.SUBSHEET_NAME)
  if not subSheets or #(subSheets) == 0 then
    return nil
  end

  for index = 1, #subSheets do
    local subSheet = subSheets[index]
    local startId = subSheet[ConfigConst.SUBSHEET_STARTID_NAME]
    local endId = subSheet[ConfigConst.SUBSHEET_ENDID_NAME]

    if dataId >= startId and dataId <= endId then
      local data = self:GetOrLoadSubSheet(subSheet)
      if not data then
        return nil
      end

      return data:GetDataById(dataId)
    end
  end

  return nil
end

function ConfigSummarySheet:GetAllId()
  if not self or type(self) ~= "table" then
    return nil
  end
  
  local allId = rawget(self,ConfigConst.ALL_ID_NAME)
  if allId then
    return allId
  end

  local subSheets = rawget(self,ConfigConst.SUBSHEET_NAME)
  if not subSheets or #(subSheets) == 0 then
    return nil
  end

  allId = {}

  for index = 1, #subSheets do
    local subSheet = subSheets[index]
    local data = self:GetOrLoadSubSheet(subSheet)

    if data then
      local ids = rawget(data,ConfigConst.ALL_ID_NAME)
      if ids then
        for k = 1, #ids do
          table.insert(allId, ids[k])
        end
      end
    end
  end

  return allId
end

ConfigSummarySheet.__index = ConfigSummarySheet
ConfigSummarySheet.__newindex = function(t,k,v)
  error("Read only,so you can't add new key for it")
end
ConfigSummarySheet.__metatable = "locked"