var currentGame = 0;
$(function () {
    
    LoadGames();

    // TODO: Not yet working
    $("#alien1").css("position", "absolute").animate({ "left": "200px" }, "slow");
    $("#alien2").css("position", "absolute").animate({ "left": "200px" }, "slow");
    $("#alien3").css("position", "absolute").animate({ "left": "200px" }, "slow");

    $("#btn-down").click(function () {

        if (currentGame + 1 >= games.length)
            currentGame = 0;
        else
            currentGame++;
      
        DrawGameMarker(currentGame);
        
    });

    $("#btn-up").click(function () {

        if (currentGame - 1 < 0)
            currentGame = games.length-1;
        else
            currentGame--;

        DrawGameMarker(currentGame);
    });

});

function LoadGames() {

    games = games.sort(function (a, b) {
        return a.name.localeCompare(b.name);
    });

    RenderGameList();
}

function RenderGameList() {
    var counter = 0;
    $.each(games, function (key, value) {
        if (currentGame == counter)
            $("#gameslist").append($('<li/>', { text: value.name, "id": "game" + counter , "class": "selected" }));
        else 
            $("#gameslist").append($('<li/>', { text: value.name, "id": "game" + counter}));
            
        counter++;
    });
    SetGame(currentGame);
}

function DrawGameMarker(selectedGame) {
    $('[id^="game"]').removeClass("selected");
    $("#game" + selectedGame).addClass("selected");
    SetGame(selectedGame);
}

function SetGame(gameid) {
    $("#game-name").text(games[gameid].name);
    $("#game-desc").text(games[gameid].desc);
    $("#game-players").text(games[gameid].players);
    $("#game-publisher").text(games[gameid].publisher);
    $("#game-year").text(games[gameid].year);
    $("#game-image").attr("src", "images/" + games[gameid].cover);
    $("#rom").val(games[gameid].rom);
}

