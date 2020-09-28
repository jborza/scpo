/*along with SignalR script to update the page and send messages*/
var hub;

function makeCards() {
    $("#cards").empty();
    var allowedValues = [0.5, 1, 2, 3, 5, 8, 13, 21, 34, 55];
    for (var i = 0; i < allowedValues.length; i++) {
        var value = allowedValues[i];
        var d = $("<div/>");
        d.attr('data-value', value);
        d.click(card_clicked);
        d.attr('id', "card-" + value);
        d.addClass("card");
        d.text(value);
        $("#cards").append(d);
    }
}

function hideMyVote() {
    forAllCards(function (f) { f.removeClass("voted"); });
}

function card_clicked() {
    vote($(this).attr('data-value'));
}

function vote(value) {
    hideMyVote();
    hub.server.castVote(roomId(), myId(), value);
    $("#card-" + value).addClass("voted");
}

// f is function(card)
function forAllCards(f) {
    $(".card").each(function () { f($(this)); });
}

function myId() {
    return $('#userid').val();
}

function roomId() {
    return $("#RoomID").val();
}

//status is a dictionary userid->vote
function updateStatus(status) {
    log("Received status:" + JSON.stringify(status));
    $("#votes").empty();
    $('#result').text("");
    for (var user in status) {
        var d = $("<div/>");
        d.addClass("vote_card");
        if (status[user]) { 
            d.addClass("vote_card_voted");
        }
        d.attr('data-value', status[user]);
        $("#votes").append(d);
    }
}

function revealVotes() {
    log("reveal votes");
    $(".vote_card").each(function() {
        $(this).text($(this).attr('data-value'));
    });
}

function hideVotes() {
    log("hide votes");
    $(".vote_card").each(function() {
        $(this).text('');
    });
};

function log(msg) {
    //to enable logging, uncomment this:
    //$('#log').append('<li><strong>' + msg + '</strong></li>');
}

function reset() {
    if (window.confirm("Are you sure you want to reset this room?")) {
        hub.server.resetRoom(roomId());
    }
}

function initialize() {
    hub.server.addClient(roomId(), myId());
    makeCards();
}

//summary is a number - the average vote
function showSummary(summary) {
    $('#result').text(summary);
    log("show summary: " + summary);
}

$(function () {
    //hide all cards
    forAllCards(function (f) { f.hide(); });
    // Reference the auto-generated proxy for the hub.
    hub = $.connection.scrumPokerHub;
    // connect proxy functions to implementations
    hub.client.showSummary = showSummary;
    hub.client.revealVotes = revealVotes;
    hub.client.hideVotes = hideVotes;
    hub.client.updateStatus = updateStatus;
    hub.client.hideClientVote = hideMyVote;

    // Start the connection and set up the callback
    $.connection.hub.start().done(function () {
        // Get the user id and store it.
        $('#userid').val($.connection.hub.id);
        initialize();
    });
    $("#resetBtn").click(reset);
    $("#revealBtn").click(function () {
        hub.server.forceReveal(roomId());
    });
});