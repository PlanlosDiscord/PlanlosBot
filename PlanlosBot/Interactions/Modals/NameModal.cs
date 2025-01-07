using Discord;
using Discord.Interactions;
using PlanlosBot.Attributes;

namespace PlanlosBot.Interactions.Modals;

public class NameModal : InteractionModuleBase
{
    [OnReady]
    public void OnUserJoined()
    {
        
    }

    [ModalInteraction("name_modal")]
    public async Task OnUserEnteredName(Modal modal)
    {
        
    }
}