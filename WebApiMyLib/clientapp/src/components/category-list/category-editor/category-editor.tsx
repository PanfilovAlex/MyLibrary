import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import TextField from "@mui/material/TextField";
import { ChangeEvent, useCallback, useMemo, useState } from "react";
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
  const [nameValidationMessage, setNameValidationMessage] = useState<string>();

  const handleValidationMessage = useCallback((key: string, messages: string[]) => {
    const keyToHandler = new Map([
      ['Name', setNameValidationMessage],
    ]);
    const handler = keyToHandler.get(key);
    if (handler) {
      handler(messages.join(', \n'));
    }
  }, [setNameValidationMessage]);

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
    }).then(async (response) => {
      return {
          ok: response.ok,
          status: response.status,
          data: await response.json(),
      };
    }).then((result) => {
      if (result.ok) {
        onSubmit && onSubmit(result.data as Category);
      } else if (result.status === 400) {
          const messages = result.data.message;
          Object.entries(messages).forEach(([k, v]) => handleValidationMessage(k, v as string[]));
      } else {
          return Promise.reject(result.data);
      }
    }).catch((error) => {
      console.error(error);
    });
  };

const onNameChange = (event: ChangeEvent<HTMLInputElement>) => {
    setName(event.target.value);
    setNameValidationMessage(undefined);
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
          error={!!nameValidationMessage}
          helperText={nameValidationMessage}
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
