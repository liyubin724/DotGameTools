require("Dot/Config/ConfigTextSheet")

local cofing_Text = {
    [1] = {
        ID = 1,
        zh = [[今天天气不错]],
        en = [[Today]],
    },
    [2] = {
        ID = 2,
        zh = [[今天天气不好]],
        en = [[Yesterday]],
    },
}
setmetatable(cofing_Text,ConfigTextSheet)
return cofing_Text
