const BoardCell = (props) => {
  return (
    <div className="cell" style={{ backgroundColor: `${props.color}` }}></div>
  );
};

export default BoardCell;
