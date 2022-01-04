import TextField from '@mui/material/TextField';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { ChangeEvent, useMemo, useState } from "react";
import { Author } from "../../../models/author";

export type AuthorEditorProps = {
    author?: Author;
    onClose: () => void;
    onSubmit: (author: Author) => void;
}

export function AuthorEditor(props: AuthorEditorProps): JSX.Element {
    const {
        author,
        onClose,
        onSubmit,
    } = props;

    const [firstName, setFirstName] = useState(author?.firstName);
    const [lastName, setLastName] = useState(author?.lastName);

    const dialogTitle = useMemo(() => {
        return author ? 'Edit' : 'Create';
    }, [author]);

    const handleSubmitButtonClick = () => {
        // TODO: wrap fetch calls in api client
        fetch('api/author', {
          method: author?.id ? 'PUT' : 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            id: author?.id,
            firstName,
            lastName,
          }),
        })
          .then((response) => response.json())
          .then((data) => {
            onSubmit && onSubmit(data as Author);
          });
      };

    const onFirstNameChange = (event: ChangeEvent<HTMLInputElement>) => {
        setFirstName(event.target.value);
    };

    const onLastNameChange = (event: ChangeEvent<HTMLInputElement>) => {
        setLastName(event.target.value);
    };

    return (
        <Dialog
            open={true}
            onClose={onClose}>

            <DialogTitle>{dialogTitle}</DialogTitle>

            <DialogContent>

                <TextField
                    label="First Name"
                    fullWidth
                    variant="standard"
                    value={firstName}
                    onChange={onFirstNameChange}
                />

                <TextField
                    label="Last Name"
                    fullWidth
                    variant="standard"
                    value={lastName}
                    onChange={onLastNameChange}
                />
            </DialogContent>

            <DialogActions>
                <Button onClick={onClose}>Cancel</Button>
                <Button onClick={handleSubmitButtonClick}>Submit</Button>
            </DialogActions>
        </Dialog>
    );
}
