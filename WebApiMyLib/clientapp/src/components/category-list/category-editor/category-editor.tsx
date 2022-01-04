import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import TextField from "@mui/material/TextField";
import { ChangeEvent, useMemo, useState } from "react";
import { Category } from "../../../models/category";

export type CategoryEditorProps = {
  category?: Category;
  onClose?: () => void;
  onSubmit?: (category: Category) => void;
};

export function CategoryEditor(props: CategoryEditorProps): JSX.Element {
  const {
    category,
    onClose,
    onSubmit,
  } = props;

  const [name, setName] = useState(category?.name);

  const dialogTitle = useMemo(() => {
    return category ? 'Edit' : 'Create';
  }, [category]);

  const handleSubmitButtonClick = () => {
    // TODO: wrap fetch calls in api client
    fetch('api/category', {
      method: category?.id ? 'PUT' : 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        id: category?.id,
        name,
      }),
    })
      .then((response) => response.json())
      .then((data) => {
        onSubmit && onSubmit(data as Category);
      });
  };

const onNameChange = (event: ChangeEvent<HTMLInputElement>) => {
    setName(event.target.value);
};

  return (
    <Dialog
      open={true}
      onClose={onClose}>
      <DialogTitle>{dialogTitle}</DialogTitle>
      <DialogContent>
        <TextField
          label="Name"
          fullWidth
          variant="standard"
          value={name}
          onChange={onNameChange}
        />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSubmitButtonClick}>Submit</Button>
      </DialogActions>
    </Dialog>
  );
}
