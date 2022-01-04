import { useCallback, useEffect, useState } from 'react';
import { Book } from '../../models/book';
import {
    Button,
    IconButton,
    List,
    ListItem,
    ListItemText,
    Pagination,
    PaginationItem,
} from '@mui/material';
import { Link, useLocation } from 'react-router-dom';
import DeleteIcon from '@mui/icons-material/Delete';
import { BookEditor } from './book-editor/book-editor';

export function BookList(): JSX.Element {
    const pageSize = 10;
    const [books, setBooks] = useState<Book[]>([]);
    const [totalCount, setTotalCount] = useState<number>(0);
    const [bookForEditing, setBookForEditing] = useState<Book>();
    const [isEditorOpened, setIsEditorOpened] = useState<boolean>(false);

    const location = useLocation();
    const query = new URLSearchParams(location.search);
    const pageNumber = parseInt(query.get('pageNumber') || '1', 10);
    const numberOfPages = Math.ceil(totalCount / pageSize);

    useEffect(() => {
        const url = `api/book?pageNumber=${pageNumber}&pageSize=${pageSize}`;
        fetch(url)
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
                console.error(`error occurred while fetching books, url: ${url}`);
            })
            .then((data) => {
                setBooks(data.items as Book[]);
                setTotalCount(data.totalCount);
            });
    }, [pageNumber, pageSize]);

    const handleItemClick = (book: Book) => {
        setBookForEditing(book);
        setIsEditorOpened(true);
    };

    const handleCreateButtonClick = () => {
        setIsEditorOpened(true);
    };

    const handleEditorClose = () => {
        setBookForEditing(undefined);
        setIsEditorOpened(false);
    };

    const handleEditorSubmit = (book: Book) => {
        setBooks((current) => {
            if (current.find((b) => b.id === book.id)) {
                return [...current.filter((b) => b.id !== book.id), book];
            }
            return [...current, book];
        });
        setIsEditorOpened(false);
    };

    const handleDeleteButtonClick = useCallback((id: number) => {
        const url = `api/book/${id}`;
        fetch(url, {
            method: 'DELETE',
        })
            // TODO: should respond with status code
            //   .then((response) => response.json())
            .then((data) => {
                if (data.ok) {
                    setBooks((current) => {
                        return current.filter((book) => book.id !== id);
                    })
                } else {
                    // TODO: add appropriate error logging
                    console.error(`error occurred while deleting book, url: ${url}`);
                }
            });
    }, [setBooks]);

    return (
        <>
            <List>
                {
                    books.map((book) => {
                        return (
                            <ListItem
                                key={book.id}>

                                <ListItemText
                                    primary={book.title}
                                    secondary={book.authors.map((a) => `${a.firstName} ${a.lastName}`).join(', ')}
                                    // secondary={<BookAuthorListEditor authors={book.authors} readonly />}
                                    onClick={() => handleItemClick(book)}
                                />

                                <IconButton
                                    color="primary"
                                    onClick={() => handleDeleteButtonClick(book.id!)}>
                                    <DeleteIcon />
                                </IconButton>
                            </ListItem>
                        );
                    })
                }
                <ListItem>
                    <Button
                        variant="text"
                        onClick={handleCreateButtonClick}>
                        Add Book
                    </Button>
                </ListItem>
            </List>

            {numberOfPages > 1 && (
                <Pagination
                    page={pageNumber}
                    count={numberOfPages}
                    renderItem={(item) => (
                        <PaginationItem
                            component={Link}
                            to={`/books${item.page === 1 ? '' : `?pageNumber=${item.page}`}`}
                            {...item} />
                    )}
                />
            )}

            {isEditorOpened && (
                <BookEditor
                    book={bookForEditing}
                    onClose={handleEditorClose}
                    onSubmit={handleEditorSubmit} />
            )}
        </>
    );
}
