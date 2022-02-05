import TextField from '@mui/material/TextField';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import Dialog from "@mui/material/Dialog";
import DialogContent from "@mui/material/DialogContent";
import DialogTitle from "@mui/material/DialogTitle";
import { ChangeEvent, useCallback, useMemo, useState } from "react";
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
    const [firstNameValidationMessage, setFirstNameValidationMessage] = useState<string>();
    const [lastName, setLastName] = useState(author?.lastName);
    const [lastNameValidationMessage, setLastNameValidationMessage] = useState<string>();

    const handleValidationMessage = useCallback((key: string, messages: string[]) => {
        const keyToHandler = new Map([
            ['First Name', setFirstNameValidationMessage],
            ['Last Name', setLastNameValidationMessage],
        ]);
        const formattedMessage = messages.join(', \n');
        const handler = keyToHandler.get(key);
        if (handler) {
            handler(formattedMessage);
        }
    }, [setFirstNameValidationMessage, setLastNameValidationMessage]);

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
        }).then(async (response) => {
            return {
                ok: response.ok,
                status: response.status,
                data: await response.json(),
            };
        }).then((result) => {
            if (result.ok) {
                onSubmit && onSubmit(result.data as Author);
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

    const onFirstNameChange = (event: ChangeEvent<HTMLInputElement>) => {
        setFirstName(event.target.value);
        setFirstNameValidationMessage(undefined);
    };

    const onLastNameChange = (event: ChangeEvent<HTMLInputElement>) => {
        setLastName(event.target.value);
        setLastNameValidationMessage(undefined);
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
                    error={!!firstNameValidationMessage}
                    helperText={firstNameValidationMessage}
                    onChange={onFirstNameChange}
                />

                <TextField
                    label="Last Name"
                    fullWidth
                    variant="standard"
                    value={lastName}
                    error={!!lastNameValidationMessage}
                    helperText={lastNameValidationMessage}
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
