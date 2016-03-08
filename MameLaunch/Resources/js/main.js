var currentGame = 0;

$(function () {
    LoadGames();

    $("#btn-down").click(function () {

        if (currentGame + 1 >= games.length)
            currentGame = 0;
        else
            currentGame++;
      
        $("#gameslist").empty();
        RenderGameList();
        
    });

    $("#btn-up").click(function () {

        if (currentGame - 1 < 0)
            currentGame = games.length-1;
        else
            currentGame--;

        $("#gameslist").empty();
        RenderGameList();
    });

});

function LoadGames() {
    RenderGameList();
    SetGame(currentGame);
}
function RenderGameList() {
    var counter = 0;
    $.each(games, function (key, value) {
        if (currentGame == counter)
            $("#gameslist").append('<li class=selected>' + value.name + '</li>');
        else
            $("#gameslist").append('<li>' + value.name + '</li>');

        counter++;

    });
}

function SetGame(gameid) {
    $("#players").text(games[gameid].players);
    $("#publisher").text(games[gameid].publisher);
}