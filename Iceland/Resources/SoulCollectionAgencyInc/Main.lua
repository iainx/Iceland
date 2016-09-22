import ("Iceland")

game:LoadMap ("pond.tmx")

ItemFactory.CreateItem ("Hood.json")
ItemFactory.CreateItem ("Skirt.json")
ItemFactory.CreateItem ("Jacket.json")

CharacterFactory.CreateCharacter ("Skeleton.json");

function SkeletonConversationResponses ()
    responses = {}
    i = 1

    if introduced == nil then
        responses[i] = 2
        i = i + 1
    end

    if gaveHood == nil and questAccepted == nil then
        responses[i] = 4
        i = i + 1
    elseif inventory:ContainsItemNamed('hood') then
        responses[i] = 9
        i = i + 1
    elseif questAccepted == true then
        responses[i] = 11
        i = i + 1
    end

    responses[i] = 8

    return responses
end

function DoNothingFunction ()
end

function DoHoodFunction ()
end

function DoJacketFunction ()
end

function DoSkirtFunction ()
end