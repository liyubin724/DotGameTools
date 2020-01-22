local getData = function(t,dataId)
  if not t or type(t) ~= "table" or not dataId or type(dataId) ~= "number" then
    return nil
  end

  local subSheets = t.subSheets
  if not subSheets then
    return nil
  end

  for index = 1, #subSheets do
    local subSheet = subSheets[index]
    local startId = subSheet.startID
    local endId = subSheet.endID

    if dataId>=startId and dataId<=endId then
      local data = subSheet.data
      if not data then
        data = require(subSheet.path)
        subSheet.data = data
      end

      if not data then
        return nil
      end

      return data[dataId]
    end
  end

  return nil
end

local getText = function(t,textId)
  if not t or type(t) ~= "table" or not textId or type(textId) ~= "number" then
    return nil
  end

  local depends = t.depends
  if not depends then
    return nil
  end

  local text = depends.text
  if not text then
    return nil
  end

  return text[textId].zh
end

local getIds = function(t)
  if not t or type(t) ~= "table" then
    return nil
  end

  local subSheets = t.subSheets
  if not subSheets then
    return nil
  end

  local allIds = {}

  for index = 1, #subSheets do
    local subSheet = subSheets[index]
    local data = subSheet.data
    if not data then
      data = require(subSheet.path)
      subSheet.data = data
    end

    if data then
      local ids = data.ids
      if ids then
        for k = 1, #ids do
          table.insert(allIds, ids[k])
        end
      end
    end
  end

  if #(allIds) > 50 then
    error("You can't call getIDs because of the count is too large")
  end

  return allIds
end


ConfigSummarySheet = {
  GetData = getData,
  GetText = getText,
  GetIds = getIds,
}

ConfigSummarySheet.__index = function(t,k)
  local value = ConfigSummarySheet[k]
  if value then
    return value
  end
end

ConfigSummarySheet.__newindex = function(t,k,v)
  error("Read only,so you can't add new key for it")
end

ConfigSummarySheet.__metatable = "locked"