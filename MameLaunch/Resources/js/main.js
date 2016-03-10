var currentGame = 0;

$(function () {
    LoadGames();

    // TODO : Clean up how this works, recreating each time isn't great
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

    // sort
    games = games.sort(function (a, b) {
        return a.name.localeCompare(b.name);
    });
    console.log(games);
    RenderGameList();
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
    SetGame(currentGame);
}

function SetGame(gameid) {
    $("#game-name").text(games[gameid].name);
    $("#game-desc").text(games[gameid].desc);
    $("#game-players").text(games[gameid].players);
    $("#game-publisher").text(games[gameid].publisher);
    $("#game-year").text(games[gameid].year);
    $("#rom").val(games[gameid].rom);
    $("#game-image").attr("src", "images/" + games[gameid].cover);
}