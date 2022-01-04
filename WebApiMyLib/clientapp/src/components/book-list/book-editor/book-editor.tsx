import { Book } from '../../../models/book';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Dialog from '@mui/material/Dialog';
import DialogActions from '@mui/material/DialogActions';
import DialogContent from '@mui/material/DialogContent';
import DialogTitle from '@mui/material/DialogTitle';
import { ChangeEvent, useMemo, useState } from 'react';
import { Category } from '../../../models/category';
import { Author } from '../../../models/author';
import { BookAuthorListEditor } from '../book-author-list-editor/book-author-list-editor';
// import styles from './book-editor.css';
import clsx from 'clsx';

export type BookEditorProps = {
  book?: Book,
  onClose?: () => void,
  onSubmit?: (book: Book) => void,
};

export function BookEditor(props: BookEditorProps): JSX.Element {
  const {
    book,
    onClose,
    onSubmit,
  } = props;

  const [title, setTitle] = useState<string>(book?.title ?? '');
  const [authors, setAuthors] = useState<Author[]>(book?.authors ?? []);
  const [categories, setCategories] = useState<Category[]>(book?.categories ?? []);

  const dialogTitle = useMemo(() => {
    return book ? 'Edit' : 'Create';
  }, [book]);

  const handleSubmitButtonClick = () => {
    // TODO: wrap fetch calls in api client
    fetch('api/book', {
      method: book?.id ? 'PUT' : 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        id: book?.id,
        title,
        authors,
        categories,
      }),
    })
      .then((response) => response.json())
      .then((data) => {
        onSubmit && onSubmit(data as Book);
      });
      // TODO: add error handling
  };

  const onTitleChange = (event: ChangeEvent<HTMLInputElement>) => {
    setTitle(event.target.value);
  }

  const onAuthorsChange = (authors: Author[]) => {
    setAuthors((current) => {
      return authors;
    });
  }

  return (
    <Dialog
      open={true}
      onClose={onClose}>
      <DialogTitle>{dialogTitle}</DialogTitle>
      <DialogContent>
        <TextField
          label="Title"
          fullWidth
          variant="standard"
          value={title}
          onChange={onTitleChange}
        />
        <BookAuthorListEditor
          className="book-author-list-editor"
          authors={authors}
          onChange={onAuthorsChange} />
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Cancel</Button>
        <Button onClick={handleSubmitButtonClick}>Submit</Button>
      </DialogActions>
    </Dialog>
  );
}
