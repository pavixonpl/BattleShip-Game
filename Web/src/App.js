import './styles.css';
import Board from './components/Board';
import Key from './components/Key';
import { useState, useEffect } from 'react';
import axios from 'axios';

export default function App() {
  let initialArray = [];
  for (let k = 0; k < 100; k++) {
    initialArray.push({ color: 'DarkTurquoise', index: k });
  }
  const createBoardUrl =
    'http://localhost:5286/api/v1/BattleshipGame/boardPattern';

  const [initial, setInitial] = useState();
  const [outcome, setOutcome] = useState();
  const [playMatchUrl, setPlayMatchUrl] = useState('');

  //I know this solution with playerOneConstBoard is bad code, but thats not my skill set xd
  const [playerOneConstBoard, setPlayerConstOneBoard] = useState([
    ...initialArray,
  ]);
  const [playerOneBoard, setPlayerOneBoard] = useState([...initialArray]);
  const [playerTwoConstBoard, setPlayerTwoConstBoard] = useState([
    ...initialArray,
  ]);
  const [playerTwoBoard, setPlayerTwoBoard] = useState([...initialArray]);
  const [winner, setWinner] = useState('');

  const getOccupiedCells = (data) => {
    const occupiedCells = [];
    data.shipsOnBoard.forEach((ship) => {
      ship.pointsOfShip.forEach((cell) => {
        const boardIndex = cell.x - 1 + (cell.y - 1) * 10;
        occupiedCells.push(boardIndex);
      });
    });
    return occupiedCells;
  };

  const getIndexOfShot = (shot) => {
    return shot.point.x - 1 + (shot.point.y - 1) * 10;
  };

  const getCellsAfterFight = (data) => {
    const newBoardPlayer1 = [...playerOneConstBoard];
    const newBoardPlayer2 = [...playerTwoConstBoard];
    const shotsPlayer1 = [];
    const shotsPlayer2 = [];

    const gameWinner = data.winner.player;
    if (gameWinner === 'Second') {
      data.winner.shots.forEach((shot) => {
        shotsPlayer1.push(shot);
      });
      data.loser.shots.forEach((shot) => {
        shotsPlayer2.push(shot);
      });
    } else {
      data.loser.shots.forEach((shot) => {
        shotsPlayer1.push(shot);
      });
      data.winner.shots.forEach((shot) => {
        shotsPlayer2.push(shot);
      });
    }

    for (const shot of shotsPlayer1) {
      console.log(shot);
      if (shot.result === 'Miss') {
        newBoardPlayer1[getIndexOfShot(shot)] = {
          ...newBoardPlayer1[getIndexOfShot(shot)],
          color: 'gray',
        };
      } else if (shot.result === 'Hit') {
        newBoardPlayer1[getIndexOfShot(shot)] = {
          ...newBoardPlayer1[getIndexOfShot(shot)],
          color: 'red',
        };
      } else if (shot.result === 'HitAndSunk') {
        newBoardPlayer1[getIndexOfShot(shot)] = {
          ...newBoardPlayer1[getIndexOfShot(shot)],
          color: 'purple',
        };
      }
    }

    for (const shot of shotsPlayer2) {
      if (shot.result === 'Miss') {
        newBoardPlayer2[getIndexOfShot(shot)] = {
          ...newBoardPlayer2[getIndexOfShot(shot)],
          color: 'gray',
        };
      } else if (shot.result === 'Hit') {
        newBoardPlayer2[getIndexOfShot(shot)] = {
          ...newBoardPlayer2[getIndexOfShot(shot)],
          color: 'red',
        };
      } else if (shot.result === 'HitAndSunk') {
        newBoardPlayer2[getIndexOfShot(shot)] = {
          ...newBoardPlayer2[getIndexOfShot(shot)],
          color: 'purple',
        };
      }
    }
    console.log(newBoardPlayer2);

    return { newBoardPlayer1, newBoardPlayer2, gameWinner };
  };

  useEffect(() => {
    if (!initial) {
      return;
    }

    setPlayMatchUrl(
      `http://localhost:5286/api/v1/BattleshipGame/boardPattern/${initial.id}/match`
    );

    const occupiedCellsPlayer1 = getOccupiedCells(initial.firstPlayersBoard);
    let newPlayerOneBoard = [...initialArray];

    for (const shipCell of occupiedCellsPlayer1) {
      newPlayerOneBoard[shipCell] = {
        ...newPlayerOneBoard[shipCell],
        color: 'black',
      };
    }

    setPlayerOneBoard(newPlayerOneBoard);
    setPlayerConstOneBoard(newPlayerOneBoard);
    const occupiedCellsPlayer2 = getOccupiedCells(initial.secondPlayersBoard);

    let newPlayerTwoBoard = [...initialArray];

    for (const shipCell of occupiedCellsPlayer2) {
      newPlayerTwoBoard[shipCell] = {
        ...newPlayerTwoBoard[shipCell],
        color: 'black',
      };
    }

    setPlayerTwoBoard(newPlayerTwoBoard);
    setPlayerTwoConstBoard(newPlayerTwoBoard);
  }, [initial]);

  const setBoardHandler = async () => {
    setWinner('');

    await axios.post(createBoardUrl, {}).then(
      (response) => {
        setInitial(response.data);
      },
      (error) => {
        console.log(error);
      }
    );
  };

  useEffect(() => {
    if (!outcome) {
      return;
    }

    const { newBoardPlayer1, newBoardPlayer2, gameWinner } =
      getCellsAfterFight(outcome);
    setPlayerOneBoard(newBoardPlayer1);
    setPlayerTwoBoard(newBoardPlayer2);
    setWinner(gameWinner);
    console.log(winner);
  }, [outcome]);

  const simulateGameHandler = async () => {
    await axios.post(playMatchUrl, {}).then(
      (response) => {
        setOutcome(response.data);
      },
      (error) => {
        console.log(error);
      }
    );
  };

  return (
    <div className='App'>
      {winner ? (
        <h1>{winner} player won!</h1>
      ) : (
        <h1 style={{ color: 'transparent' }}>No winner yet</h1>
      )}
      <Board boardData={playerOneBoard} />
      <Board boardData={playerTwoBoard} />
      <button onClick={setBoardHandler}>Set boards</button>
      <button onClick={simulateGameHandler}>Simulate game</button>
      <Key />
    </div>
  );
}
