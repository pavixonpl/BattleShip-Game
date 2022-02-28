import BoardCell from "./BoardCell";

const Board = (props) => {
  const cells = props.boardData.map((cell) => {
    return <BoardCell color={cell.color} key={cell.index} />;
  });

  return <div className="boardContainer"> {cells} </div>;
};

export default Board;
